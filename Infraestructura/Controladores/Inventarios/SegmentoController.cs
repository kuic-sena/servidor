﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Modelo;
using Dominio.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Infraestructura.Extensiones;

namespace Infraestructura.Controladores.Inventarios
{
    [Route("api/[controller]")]
    [Autenticado]
    public class SegmentoController : Controller
    {
        private readonly RepoSegmento repo = new RepoSegmento();

        [HttpGet]
        public IEnumerable<Segmento> Listar()
        {
            return repo.Listar();
        }

        [HttpGet("{id}")]
        public ActionResult<Segmento> Obtener(int id)
        {
            var segmento = repo.PorId(id);

            if (segmento is Segmento)
            {
                return segmento;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Segmento datos)
        {
            if (repo.Insertar(datos))
            {
                return Accepted();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Editar(int id, [FromBody]Segmento datos)
        {
            if (repo.PorId(id) is Segmento)
            {
                datos.Id = id;
                if (repo.Editar(datos))
                {
                    return Accepted();
                }
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var segmento = repo.PorId(id);
            if (segmento is Segmento)
            {
                if (repo.Eliminar(segmento))
                {
                    return Accepted();
                }
            }
            return BadRequest();
        }
    }
}