using Data.DataModel;
using Data.Uow;
using Microsoft.AspNetCore.Http;
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
    public class ContainerController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ILogger<ContainerController> _logger;


        public ContainerController(ILogger<ContainerController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var listofcontainers = unitOfWork.Container.GetAll();
            return Ok(listofcontainers);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var container = unitOfWork.Container.GetById(id);

            if (container is null)
            {
                return NotFound();
            }

            return Ok(container);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Container entity)
        {

            unitOfWork.Container.Add(entity);
            unitOfWork.Complete();

            var containers = unitOfWork.Container.GetAll();

            return Ok(containers);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] Container container, int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var model = unitOfWork.Container.GetById(id);
            model.ContainerName = container.ContainerName;

            unitOfWork.Container.Update(model);
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

            unitOfWork.Container.Delete(id);
            unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("GetContainersByVehicleId")]
        public IActionResult GetContainersByVehicleId([FromQuery] int id)
        {
            //List containers by VehicleId

            if (id == 0)
            {
                return BadRequest();
            }

            var containers = unitOfWork.Container.GetAll(x => x.VehicleID == id);
            if (containers is null)
            {
                return NotFound();
            }
            return Ok(containers);
        }
    }
}

