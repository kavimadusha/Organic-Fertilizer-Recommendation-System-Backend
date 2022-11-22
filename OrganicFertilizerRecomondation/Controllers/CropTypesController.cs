using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFertilizerRecomondation.DTOs;
using OrganicFertilizerRecomondation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrganicFertilizerRecomondation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CropTypesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // GET: api/<CropTypesController>
        [HttpGet]
        public async Task<ActionResult<List<DropDownDTO>>> Get()
        {
            var _dpList = new List<DropDownDTO>();
            try
            {
                var items = await _applicationDbContext.cropTypes.ToListAsync();
                foreach (var item in items)
                {
                    _dpList.Add(new DropDownDTO
                    {
                        Value = item.Id,
                        Label = item.Name
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return _dpList;
        }

        [HttpGet("CropTypeAges")]
        public async Task<ActionResult<List<DropDownDTO>>> CropTypeAges(int id)
        {
            var _dpList = new List<DropDownDTO>();
            try
            {
                var items = await _applicationDbContext.cropAges.Where(x=>x.CropTypeId == id).ToListAsync();
                foreach (var item in items)
                {
                    _dpList.Add(new DropDownDTO
                    {
                        Value = item.Id,
                        Label = item.Age
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return _dpList;
        }


        [HttpGet("NaturalSources")]
        public async Task<ActionResult<List<DropDownDTO>>> NaturalSources()
        {
            var _dpList = new List<DropDownDTO>();
            try
            {
                var items = await _applicationDbContext.naturalSources.ToListAsync();
                foreach (var item in items)
                {
                    _dpList.Add(new DropDownDTO
                    {
                        Value = item.Id,
                        Label = item.Source
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return _dpList;
        }
    }
}
