﻿using APIcomMongoDB.Data.Collection;
using APIcomMongoDB.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIcomMongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
            Data.MongoDB _mongoDB;
            IMongoCollection<Infectado> _infectadosCollection;

            public InfectadoController(Data.MongoDB mongoDB)
            {
                _mongoDB = mongoDB;
                _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
            }

            [HttpPost]
            public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
            {
                var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

                _infectadosCollection.InsertOne(infectado);

                return StatusCode(201, "Infectado adicionado com sucesso");
            }

            [HttpGet]
            public ActionResult ObterInfectados()
            {
                var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

                return Ok(infectados);
            }

            [HttpPut]
            public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
            {
                // atualizaçao por data de nascimento, o ideal seria que fosse por id
                //atualizando apenas uma propriedade
                _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo));

                return Ok("Atualizado com sucesso");
            }


            [HttpDelete("{dataNascimento}")]
            public ActionResult Delete(DateTime dataNascimento)
            {
                // atualizaçao por data de nascimento, o ideal seria que fosse por id
                _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dataNascimento));

                return Ok("Deletado com sucesso");
            }
    }
}
