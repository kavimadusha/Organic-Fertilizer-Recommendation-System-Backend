using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFertilizerRecomondation.DTOs;
using OrganicFertilizerRecomondation.Models;
using OrganicFertilizerRecomondation.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrganicFertilizerRecomondation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecomondationsController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public RecomondationsController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // GET: api/<RecomondationsController>
        [HttpGet]
        

        // POST api/<RecomondationsController>
        [HttpPost]
        public async Task<CompostAmountDTO> Post(FertilizerRecomondationRequest request)
        {
            var compostAmountDTO = new CompostAmountDTO();
            try
            {
                // get crop type age ID
                var cropDetails = await _applicationDbContext.cropAges.FirstOrDefaultAsync(c => c.Id == request.CropTypeAgeId); 

                //NPK requirement of the crop based on crop age
                var NRequirment = cropDetails.Nitrigion;
                var PRequirment = cropDetails.Phosphurus;
                var KRequirment = cropDetails.Pottasium;

                compostAmountDTO.NValue = NRequirment;
                compostAmountDTO.PValue = PRequirment;
                compostAmountDTO.KValue = KRequirment;

                //Deduct the soil NPK content from the NPK Requirement (devided by 10000) default value is passed as 0 
                var _a = (NRequirment != 0)? NRequirment - (request.NContentOfSoil / 10000) : 0;
                var _b = (PRequirment != 0) ? PRequirment - (request.PContentOfSoil / 10000) : 0;
                var _c = (KRequirment != 0) ? KRequirment - (request.KContentOfSoil / 10000) : 0;

                // adiung object arry to pervious calculated value and it's type N,P and K , after reduing requird npk - soil npk
                var doubles = new List<CalItem>();
                doubles.Add(new CalItem { Item = "N", Value = _a });
                doubles.Add(new CalItem { Item = "P", Value = _b });
                doubles.Add(new CalItem { Item = "K", Value = _c });

                var _detailsInCompost = doubles;

                //Order the NPK Requirement in aesending order
                doubles = doubles.OrderBy(x => x.Value).ToList();
                //Get NPK Requirement where the value is not 0
                doubles = doubles.Where(x=>x.Value != 0).ToList();
                //NPK Requirement where the value is greater than 0 
                doubles = doubles.Where(x => x.Value > 0).ToList();

                var minVal = 0;
                var fiterItem = new CalItem();

                if (doubles[0].Value != 0) // check 1st item in arry is 0, is not take value to future calculation 
                {
                    fiterItem = doubles[0];
                }
                else // if 1st value 0 then go to else 
                {
                    fiterItem = doubles[1];
                }

                //Swich to select which nutrition should be fullfilled by compost according to the minimum NPK ratio
                double _contentOfComposte = 0;
                switch (fiterItem.Item)
                {
                    case "N":
                        _contentOfComposte = request.NContentOfComposte;
                        break;
                    case "P":
                        _contentOfComposte = request.PContentOfComposte;
                        break;                        
                    case "K":
                        _contentOfComposte = request.KContentOfComposte;
                        break;
                }

                //calculating the ammount of compost
                var amountOfCompost = (fiterItem.Value * 100) / (_contentOfComposte / 10000);
                if(amountOfCompost > 0)
                {
                    //multiplying the final answer by the land area to get the quantity for the land area
                    compostAmountDTO.Amount = amountOfCompost * request.Area;
                }

                //geting the list of natural sources
                var _naturalSources = await _applicationDbContext.naturalSources.ToListAsync(); 
                var _naturalSourceAmountList = new List<NaturalSourceAmount>();

                //looping natural sources
                foreach (var source in _naturalSources) 
                {
                    var _naturalSourceAmount = new NaturalSourceAmount();

                    _naturalSourceAmount.Name = source.Source;

                    var naturalItems = new List<CalItem>();
                    //getting natural source NPK values and type using arry
                    naturalItems.Add(new CalItem { Item = "N", Value = source.Nitrigion });
                    naturalItems.Add(new CalItem { Item = "P", Value = source.Phosphurus });
                    naturalItems.Add(new CalItem { Item = "K", Value = source.Pottasium });

                    // geting other NPK that was not fulfiled, get other valus not taking to caluculte amount of compost 
                    naturalItems = naturalItems.Where(c => c.Item != fiterItem.Item).ToList();
                    _detailsInCompost = _detailsInCompost.Where(c => c.Item != fiterItem.Item).ToList();

                    // rest of valus are not usign calculte compost, check values whci are greter than 0 and take that
                    double amountNaturalSource = 0;
                    if(fiterItem.Value > 0)
                    {
                        foreach (var compost in _detailsInCompost) //for loop to get natural source amount 
                        {

                            //calculate the ammount of other nutritions in compost
                            double _currentCompostItmeAmount = 0;
                            switch (compost.Item)
                            {
                                case "N":
                                    _currentCompostItmeAmount = request.NContentOfComposte;
                                    break;
                                case "P":
                                    _currentCompostItmeAmount = request.PContentOfComposte;
                                    break;
                                case "K":
                                    _currentCompostItmeAmount = request.KContentOfComposte;
                                    break;
                            }

                            _currentCompostItmeAmount = _currentCompostItmeAmount / 10000;

                            //calculate the ammount of other nutritions in compost
                            var currntHasAmount = (amountOfCompost * _currentCompostItmeAmount) / 100;      

                            //Get the defference which is needed to be fullfilled using natural sources, required - compost fulfils
                            var _diff = compost.Value - currntHasAmount;
                            if (_diff > 0)
                            {
                                //calculate the natural source quantity which should be provided and sort it in aesending order
                                var _value = naturalItems.FirstOrDefault(x => x.Item == compost.Item).Value;
                                var cal = (_diff * 100) / _value;
                                //Check whether the remaining requirement is fillfilled, if 2 requirmnt check other one is fufiled or not after taking highest one
                                if (cal > amountNaturalSource)
                                {
                                    //if not re calculate the natural source ammount for the new gap
                                    amountNaturalSource = (cal - amountNaturalSource) + amountNaturalSource;
                                }

                            }
                        }
                    }
                    
                    //Natural source ammount multiplied by the land area to provide the quantity required for the land area
                    _naturalSourceAmount.Amount = amountNaturalSource * request.Area;

                    _naturalSourceAmountList.Add(_naturalSourceAmount);
                }

                compostAmountDTO.NaturalSourceAmounts = _naturalSourceAmountList;




            }
            catch(Exception ex)
            {

            }
            return compostAmountDTO;
        }

        [HttpPost("NaturalSourceAmount")]
        public async Task<CompostAmountDTO> NaturalSourceAmount(NaturalSourceAmountRequest request)
        {
            var compostAmountDTO = new CompostAmountDTO();
            try
            {
                var cropDetails = await _applicationDbContext.naturalSources.FirstOrDefaultAsync(c => c.Id == request.NaturalSourceId);

                
            }
            catch (Exception ex)
            {

            }
            return compostAmountDTO;
        }

        // PUT api/<RecomondationsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RecomondationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
