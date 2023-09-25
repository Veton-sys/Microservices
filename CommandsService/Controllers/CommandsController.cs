using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommmandsForPlatform {platformId}");
            if (!_repository.PlatformExists(platformId)) return NotFound();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(_repository.GetCommandsForPlatform(platformId)));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit Single GetCommmandForPlatform {platformId} / {commandId}");
            if (!_repository.PlatformExists(platformId)) return NotFound();

            var command = _repository.GetCommand(platformId, commandId);
            if (command == null) return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));

        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform {platformId}");
            if (!_repository.PlatformExists(platformId)) return NotFound();

            var command = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            //because the command is saved in db it has an Id when its getting mapped down here
            var commandRead = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = commandRead.Id}, commandRead );
        }
    }
}