using B2b_Api.Models;
using B2b_Api.Servisler;
using System.Web.Http;
using System.Web.Http.Cors;

namespace B2b_Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        private readonly CategoryService _service = new CategoryService();

        //[HttpGet]
        //[Route("list")]
        //public IHttpActionResult List()
        //{
        //    var categories = _service.GetAll();
        //    return Ok(categories);
        //}
        [Route("KategorileriOku")]
        public Sonuc GetKategorileriOku()
        {
            Sonuc sonuc = new Sonuc();
            CategoryService cs = new CategoryService();
            sonuc = cs.KategorileriOku();
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("KategoriEkle")]
        public Sonuc KategoriEkle([FromBody] Category model)
        {
            Sonuc sonuc = new Sonuc();
            CategoryService cs = new CategoryService();
            sonuc = cs.KategoriEkle(model);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("KategoriDuzenle")]
        public Sonuc KategoriDuzenle([FromBody] Category model)
        {
            Sonuc sonuc = new Sonuc();
            CategoryService cs = new CategoryService();
            sonuc = cs.KategoriGuncelle(model);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [Route("KategoriSil/{id}")]
        public Sonuc GetKategoriSil(int id)
        {
            Sonuc sonuc = new Sonuc();
            CategoryService cs = new CategoryService();
            sonuc = cs.KategoriSil(id);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [Route("KategoriAcKapa/{id}")]
        public Sonuc GetKategoriAcKapa(int id)
        {
            Sonuc sonuc = new Sonuc();
            CategoryService cs = new CategoryService();
            sonuc = cs.KategoriAcKapa(id);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }

        //[HttpPost]
        //[Route("create")]
        //public IHttpActionResult Create([FromBody] Category model)
        //{
        //    if (string.IsNullOrWhiteSpace(model.Name))
        //        return BadRequest("Kategori adı zorunludur.");

        //    if (string.IsNullOrWhiteSpace(model.Code))
        //        return BadRequest("Kategori kodu zorunludur.");

        //    if (_service.CodeExists(model.Code))
        //        return BadRequest("Bu kategori kodu zaten kullanılıyor.");

        //    if (model.ParentId.HasValue && !_service.ParentExists(model.ParentId.Value))
        //        return BadRequest("Üst kategori bulunamadı.");

        //    var created = _service.Create(model);
        //    return Ok(created);
        //}

        //[HttpPost]
        //[Route("update")]
        //public IHttpActionResult Update([FromBody] Category model)
        //{
        //    var existing = _service.GetById(model.Id);
        //    if (existing == null)
        //        return NotFound();

        //    if (string.IsNullOrWhiteSpace(model.Name))
        //        return BadRequest("Kategori adı zorunludur.");

        //    if (string.IsNullOrWhiteSpace(model.Code))
        //        return BadRequest("Kategori kodu zorunludur.");

        //    if (_service.CodeExists(model.Code, model.Id))
        //        return BadRequest("Bu kategori kodu zaten kullanılıyor.");

        //    if (model.ParentId.HasValue)
        //    {
        //        if (model.ParentId.Value == model.Id)
        //            return BadRequest("Kategori kendisinin alt kategorisi olamaz.");

        //        if (!_service.ParentExists(model.ParentId.Value))
        //            return BadRequest("Üst kategori bulunamadı.");

        //        if (_service.IsDescendant(model.Id, model.ParentId.Value))
        //            return BadRequest("Döngüsel bağımlılık oluşur.");
        //    }

        //    var updated = _service.Update(model);
        //    return Ok(updated);
        //}

        //[HttpPost]
        //[Route("delete")]
        //public IHttpActionResult Delete([FromBody] Category model)
        //{
        //    var existing = _service.GetById(model.Id);
        //    if (existing == null)
        //        return NotFound();

        //    if (_service.HasChildren(model.Id))
        //        return BadRequest("Alt kategorileri olan bir kategori silinemez.");

        //    _service.Delete(model.Id);
        //    return StatusCode(System.Net.HttpStatusCode.NoContent);
        //}

        [HttpPost]
        [Route("toggle")]
        public IHttpActionResult Toggle([FromBody] Category model)
        {
            var existing = _service.GetById(model.Id);
            if (existing == null)
                return NotFound();

            var toggled = _service.Toggle(model.Id);
            return Ok(toggled);
        }
    }
}
