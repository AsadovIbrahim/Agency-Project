using DataBase.Entities.Concretes;
using DataBase.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Agency_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuController(IPortfolioRepository portfolioRepository, IWebHostEnvironment webHostEnvironment)
        {
            _portfolioRepository = portfolioRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var data=await _portfolioRepository.GetAllAsync();
            return View(data);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult>Create(Portfolio portfolio)
        {
            if(!ModelState.IsValid)
            {
                return View(portfolio);
            }
            string path = _webHostEnvironment.WebRootPath + @"\Upload\manage\";
            string fileName = Guid.NewGuid() + portfolio.ImgFile.FileName;
            string fullPath=Path.Combine(path, fileName);

            using(FileStream stream=new FileStream(fullPath, FileMode.Create))
            {
                portfolio.ImgFile.CopyTo(stream);
            }
            portfolio.ImgUrl=fileName; 
            
            await _portfolioRepository.AddAsync(portfolio);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _portfolioRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task< IActionResult> Update(int id)
        {
            var data=await _portfolioRepository.GetByIdAsync(id);
            return View(data);
        }

        [HttpPost]

        public IActionResult Update(Portfolio portfolio)
        {
            if (!ModelState.IsValid)
            {
                return View(portfolio);
            }
            if (portfolio.ImgFile != null)
            {
                string path = _webHostEnvironment.WebRootPath + @"\Upload\manage\";
                string fileName = Guid.NewGuid() + portfolio.ImgFile.FileName;
                string fullPath = Path.Combine(path, fileName);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    portfolio.ImgFile.CopyTo(stream);
                }
                portfolio.ImgUrl = fileName;
            }
            _portfolioRepository.UpdateAsync(portfolio);
            return RedirectToAction("Index");
        }
    }
}
