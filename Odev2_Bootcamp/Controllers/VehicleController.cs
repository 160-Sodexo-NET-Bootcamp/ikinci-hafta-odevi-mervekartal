using Data;
using Data.DataModel;
using Data.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odev2_Bootcamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ILogger<VehicleController> _logger;

        public VehicleController(ILogger<VehicleController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var listofvehicles = unitOfWork.Vehicle.GetAll();
            return Ok(listofvehicles);
        }


        [HttpGet("{id}")]
        public  IActionResult GetById(int id)
        {
            var vehicle = unitOfWork.Vehicle.GetById(id);

            if (vehicle is null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Vehicle entity)
        {
            unitOfWork.Vehicle.Add(entity);
            unitOfWork.Complete();

            var vehicles = unitOfWork.Vehicle.GetAll();

            return Ok(vehicles);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] Vehicle vehicle, int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var model = unitOfWork.Vehicle.GetById(id);
            model.VehicleName = vehicle.VehicleName;
            model.VehiclePlate = vehicle.VehiclePlate;

            unitOfWork.Vehicle.Update(model);
            unitOfWork.Complete();

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            unitOfWork.Vehicle.Delete(id);
            unitOfWork.Complete();
            return Ok();
        }
    }
}
