using AlkemyChallenge.DTOs;
using AlkemyChallenge.Entities;
using AlkemyChallenge.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkemyChallenge.Controllers
{
    public class CustomBaseController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CustomBaseController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
        {
            var entitys = await context.Set<TEntity>().AsNoTracking().ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(entitys);
            return dtos;
        }


        protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDTO paginationDTO) where TEntity : class
        {
            var queryable = context.Set<TEntity>().AsQueryable();
            return await Get<TEntity, TDTO>(paginationDTO, queryable);
        }

        protected async Task<List<TDTO>> Get<Tentity, TDTO>(PaginationDTO paginationDTO,
            IQueryable<Tentity> queryable)
            where Tentity : class
        {
            await HttpContext.InsertPaginationParameters(queryable, paginationDTO.NumberEntrysPerPage);
            var entidades = await queryable.Pagination(paginationDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entidades);
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IId
        {
            var entity = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            return mapper.Map<TDTO>(entity);
        }

        protected async Task<ActionResult> Post<TCreation, TEntity, TDTO>(TCreation creationDTO, string pathName) where TEntity : class, IId
        {
            if (creationDTO == null)
                return BadRequest();

            var entity = mapper.Map<TEntity>(creationDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<TDTO>(entity);

            return new CreatedAtRouteResult(pathName, new { id = entity.Id }, dto);
        }

        protected async Task<ActionResult> Put<TCreation, TEntity>(int id, TCreation creationDTO) where TEntity: class, IId
        {
            var entity = mapper.Map<TEntity>(creationDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        } 

        protected async Task<ActionResult> Delete<TEntity>(int id) where TEntity : class, IId
        {
            var entity = await context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                return NotFound();

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Patch<TEntity, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument)
            where TDTO : class
            where TEntity : class, IId
        {
            if (patchDocument == null)
                return BadRequest();

            var entityDb = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

            if (entityDb == null)
                return NotFound();

            var entityDTO = mapper.Map<TDTO>(entityDb);

            patchDocument.ApplyTo(entityDTO, ModelState);

            var isValid = TryValidateModel(entityDTO);

            if (!isValid)
                return BadRequest(ModelState);

            mapper.Map(entityDTO, entityDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
