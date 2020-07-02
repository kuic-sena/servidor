﻿using Aplicacion.Modelo.Inventarios;
using Aplicacion.Servicio.Inventarios;
using Dominio.Modelo;
using Dominio.Repositorio;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

namespace Infraestructura.Controladores.Inventarios
{
    [Route("api/[controller]")]
    [Authorize(Roles = "logistica")]
    public class EntradaController : Controller
    {
        private readonly RepoEntrada repositorio = new RepoEntrada();

        [HttpGet]
        public IEnumerable<Entrada> Listar()
        {
            return repositorio.Listar();
        }

        [HttpGet("{id}")]
        public ActionResult<Entrada> Obtener(int id)
        {
            Entrada entrada = repositorio.PorId(id);
            if (entrada is Entrada)
            {
                return entrada;
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] FormularioRegistrarEntrada formulario)
        {
            var servicio = new ServicioRegistradorEntrada();

            if (servicio.Registrar(formulario))
            {
                return Accepted();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var entrada = repositorio.PorId(id);

            if (entrada is Entrada)
            {
                var servicioEliminador = new ServicioEliminadorEntrada();

                if (servicioEliminador.Eliminar(entrada))
                {
                    return Accepted();
                }
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Editar(int id, [FromBody] Entrada informacion)
        {
            if (repositorio.PorId(id) is Entrada)
            {
                informacion.Id = id;

                if (repositorio.Editar(informacion))
                {
                    return Accepted();
                }
            }

            return BadRequest();
        }
    }
}
