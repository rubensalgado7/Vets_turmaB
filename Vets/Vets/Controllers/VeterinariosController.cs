using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vets.Controllers.Data;
using Vets.Models;

namespace Vets.Controllers
{
    public class VeterinariosController : Controller
    {
        /// <summary>
        /// variavel que identifica a BD do nosso projeto
        /// </summary>
        private readonly VetsDB _context;

        /// <summary>
        /// variavel que contem os dados do ambiente do servisdor
        /// em particular, ondes estao os ficheiros guardados, no disco rigido do servidor 
        /// </summary>
        private readonly IWebHostEnvironment _caminho; 

        public VeterinariosController(VetsDB context,IWebHostEnvironment caminho)
        {
            _context = context;
            _caminho = caminho;
        }

        // GET: Veterinarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Veterinarios.ToListAsync());
        }

        /// <summary>
        /// Mostra os detalhes de um veterinario
        /// Se houverem,mostra os detalhes das consultas associadas a ele
        /// Pesquisa feita em modo 'Eager Loading '
        /// </summary>
        /// <param name="id">identificador do veterinario</param>
        /// <returns></returns>
        // GET: Veterinarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios.Include(v=>v.Consultas).ThenInclude(a=>a.Animal).ThenInclude(d=>d.Dono).FirstOrDefaultAsync(v => v.ID == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        /// <summary>
        /// Mostra os detalhes de um veterinario
        /// Se houverem,mostra os detalhes das consultas associadas a ele
        /// Pesquisa feita em modo 'Lazy Loading'
        /// </summary>
        /// <param name="id">identificador do veterinario</param>
        /// <returns></returns>
        // GET: Veterinarios/Details/5
        public async Task<IActionResult> Details2(int? id)
        {
          

            var veterinarios = await _context.Veterinarios.FirstOrDefaultAsync(v => v.ID == id);
            //necessário adicionar o termo 'virtual' aos relacionamentos
            //necessario adicionar um novo pacote
            //Install EntityFramework.Proxies
            //dar ordem ao programa para usar o serviço
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // GET: Veterinarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veterinarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinario,IFormFile fotoVet)
        {
            //processar a fotografia
            string caminhoCompleto = "";
            bool haImagem = false;

                // processar a fotografia -> se nao ha usamos noVet.jog que é a default para todos
            if (fotoVet == null)
            {
                veterinario.Fotografia = "noVet.jpg";
            }
            else
            {
                //sera o ficheiro uma imagemc
                if (fotoVet.ContentType == "image/jpeg" || fotoVet.ContentType == "imahe/png")
                {

              
                    //o ficheiro é uma imagem valida
                    //prepara a imagem para ser guardada no disco e o seu nome associado ao vet
                    Guid g;
                    g = Guid.NewGuid();
                    string extensao = Path.GetExtension(fotoVet.FileName).ToLower();
                    string nome = g.ToString() + extensao;
                    //onde guardar o ficheiro
                    caminhoCompleto = Path.Combine(_caminho.WebRootPath, "Imagens\\Vets", nome);
                    //associar o nome do ficheiro ao vet
                    veterinario.Fotografia = nome;
                    //assinalar que existe imagem e é preciso guarda-la no disco
                    haImagem = true;

                }
                else
                {
                    //ha imagem mas nao é do tipo certo
                    veterinario.Fotografia = "noVet.jpg";
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(veterinario);
                    await _context.SaveChangesAsync();
                    // se há imagem, vou guardar no disco
                    if (haImagem)
                    {
                        using var stream = new FileStream(caminhoCompleto, FileMode.Create);
                        await fotoVet.CopyToAsync(stream);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    //se chegar aqui, é pq alguma coisa correu mal
                    //o que fazer?

                }
                }
            return View(veterinario);

            
        }

        // GET: Veterinarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios.FindAsync(id);
            if (veterinario == null)
            {
                return NotFound();
            }
            return View(veterinario);
        }

        // POST: Veterinarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinario,IFormFile fotoVet)
        {
           

            if (id != veterinario.ID)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veterinario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinariosExists(veterinario.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(veterinario);
        }

        // GET: Veterinarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.ID == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // POST: Veterinarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veterinarios = await _context.Veterinarios.FindAsync(id);
            _context.Veterinarios.Remove(veterinarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeterinariosExists(int id)
        {
            return _context.Veterinarios.Any(e => e.ID == id);
        }
    }
}
