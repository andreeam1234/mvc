using System.Diagnostics;
using Lab06.Models;
using Lab06.Services;
using Lab06.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lab06.Controllers;

public class HomeController : Controller
{
    private readonly IArticleService _articleService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IArticleService articleService,
        ICategoryService categoryService,
        ILogger<HomeController> logger)
    {
        _articleService = articleService;
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var allArticles = await _articleService.GetAllAsync(cancellationToken);

        var recent = allArticles
            .OrderByDescending(a => a.PublishedAt)
            .Take(3)
            .Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CategoryName = a.Category?.Name ?? "N/A"
            }).ToList();

        var vm = new HomeViewModel
        {
            RecentArticles = recent,
            TotalArticles = await _articleService.CountAsync(cancellationToken),
            TotalCategories = await _categoryService.CountAsync(cancellationToken)
        };

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}