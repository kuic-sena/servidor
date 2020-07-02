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
    public class SalidaController : Controller
    {
        private readonly RepoSalida repo = new RepoSalida();

        [HttpGet]
        public IEnumerable<Salida> Listar()
        {
            return repo.Listar();
        }

        [HttpGet("{id}")]
        public ActionResult<Salida> Obtener(int id)
        {
            Salida salida = repo.PorId(id);

            if (salida is Salida)
            {
                return salida;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] FormularioRegistrarSalida formulario)
        {
            var servicio = new ServicioRegistradorSalida();

            if (servicio.Registrar(formulario))
            {
                return Accepted();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Editar(int id, [FromBody] Salida datos)
        {
            if (repo.PorId(id) is Salida)
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
            Salida salida = repo.PorId(id);
            if (salida is Salida)
            {
                if (repo.Eliminar(salida))
                {
                    return Accepted();
                }
            }
            return BadRequest();
        }
    }
}
