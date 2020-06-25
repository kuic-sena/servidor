﻿using Aplicacion.Modelo.Sesiones;
using Aplicacion.Servicio.Usuarios;

using Blazored.LocalStorage;

using Dominio.Modelo;

using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infraestructura.Autenticacion
{
    public sealed class BlazorAuthenticationProvider: AuthenticationStateProvider
    {
        private readonly HttpClient http;
        private readonly ILocalStorageService localStorage;

        public BlazorAuthenticationProvider(HttpClient http, ILocalStorageService localStorage)
        {
            this.http = http;
            this.localStorage = localStorage;
        }

        private async Task<AuthenticationState> ObtenerIdentidad()
        {
            AuthenticationState estado = new AuthenticationState(new ClaimsPrincipal());

            var token = await localStorage.GetItemAsync<string>("token");

            if (token is string)
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                var respuesta = await http.GetAsync("/api/sesion");

                if (respuesta.IsSuccessStatusCode)
                {
                    var usuario = await respuesta.Content.ReadFromJsonAsync<Usuario>();
                    var servicio = new ServicioSesion();

                    var identidad = servicio.GenerarIdentidad(usuario);
                    estado = new AuthenticationState(identidad);
                }
            }

            return estado;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                return await ObtenerIdentidad();
            } 

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public async Task IniciarSesion(string documento, string clave)
        {
            var credencial = new Credencial(documento, clave);
            var respuesta = await http.PostAsJsonAsync("/api/sesion", credencial);

            if (respuesta.IsSuccessStatusCode)
            {
                string token = await respuesta.Content.ReadAsStringAsync();
                await localStorage.SetItemAsync("token", token);

                NotifyAuthenticationStateChanged(ObtenerIdentidad());
            }
        }

        public async Task CerrarSesion()
        {
            await localStorage.ClearAsync();
            NotifyAuthenticationStateChanged(ObtenerIdentidad());
        }
    }
}