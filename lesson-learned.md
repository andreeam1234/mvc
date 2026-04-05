Explicarea structurii proiectului
1. Controllers

Controllers sunt punctul de intrare in aplicatie. Ei primesc request-uri HTTP de la utilizator (ex: deschiderea unei pagini sau trimiterea unui formular) si returneaza un raspuns (de obicei un View).
Controller-ul nu trebuie sa contina logica complexa. El doar:
- apeleaza service-uri
- trimite date catre View
- gestioneaza fluxul aplicatiei

2.Services

Services contin logica de business a aplicatiei. Ele decid ce trebuie facut cu datele.

Aici se pun:
- validari suplimentare
- reguli de business
- combinarea datelor din mai multe surse
Service-ul face legatura intre Controller si Repository.

3. Repositories

Repositories se ocupa strict de accesul la baza de date.
Ele folosesc DbContext si contin operatii precum:
- GetAll
- GetById
- Add
- Update
- Delete
Avantajul este ca izoleaza logica de acces la date de restul aplicatiei.

4. ViewModels

ViewModels sunt clase folosite pentru a trimite date catre View.
Ele nu sunt aceleasi cu modelele din baza de date. Contin doar datele necesare pentru afisare sau pentru formular.

Exemplu:
ArticleViewModel (pentru afisare)
CreateArticleViewModel (pentru formular de creare)
EditArticleViewModel (pentru editare)

5. Views

Views sunt partea vizuala a aplicatiei (HTML + Razor).
Ele afiseaza datele primite de la Controller si contin formulare pentru input de la utilizator.
View-urile nu contin logica de business, doar afisare.

Intrebari:

- De ce folosim Repository Pattern?

Folosim Repository Pattern pentru a separa logica de acces la baza de date de restul aplicatiei. In loc sa lucram direct cu DbContext peste tot, avem un strat dedicat care se ocupa doar de operatii pe date (CRUD).
Astfel, codul devine mai organizat, mai usor de intretinut si mai usor de modificat daca schimbam baza de date sau modul de acces la date.

- Ce s-ar intampla daca apelam _context direct din controller?

Daca apelam _context direct din controller:
- controller-ul devine incarcat cu logica de acces la date
- codul devine greu de citit si de mentinut
- nu mai exista separare clara intre responsabilitati
- este mai greu de testat (nu putem face mock usor la DbContext)
Practic, controller-ul ar face prea multe lucruri si ar incalca principiul Single Responsibility.

- De ce avem un Service Layer separat?

Service Layer contine logica de business a aplicatiei. El face legatura intre controller si repository.
Controller-ul doar primeste request-ul si returneaza raspunsul, iar Service Layer decide ce trebuie facut efectiv (validari, transformari, reguli de business).

- Ce logica ar ajunge in controller fara el?

Fara Service Layer, in controller ar ajunge:
- validari suplimentare
- logica de creare/modificare a entitatilor
- combinarea datelor din mai multe tabele
- reguli de business (ex: verificari, calcule)
Asta ar face controller-ul foarte mare si greu de intretinut.

- De ce folosim interfete (IArticleRepository, IArticleService)?

Folosim interfete pentru:
- a permite Dependency Injection
- a face codul mai flexibil si usor de schimbat
- a putea testa mai usor (mock-uri)
Controller-ul nu depinde de implementare concreta, ci de interfata.
Exemplu concret din cod:
In ArticlesController, nu cerem clasa ArticleService, ci interfata:

private readonly IArticleService _articleService;

public ArticlesController(IArticleService articleService) {
    _articleService = articleService;
}
Acest lucru ne permite sa schimbam implementarea (de exemplu, pentru teste unitare) fara sa modificam controllerul.

- - Cum ajuta aceasta structura pentru un API REST sau aplicatie mobila?

Aceasta structura este foarte utila pentru extindere.
Pentru un API REST:
- putem crea un nou controller API care foloseste aceleasi servicii
- nu trebuie sa rescriem logica de business
Pentru o aplicatie mobila:
- backend-ul (service + repository) ramane acelasi
- doar frontend-ul se schimba (ex: aplicatie mobila in loc de web)
Astfel, putem reutiliza codul si mentine aplicatia mai usor pe termen lung.