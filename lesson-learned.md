1. De ce Logout este implementat ca `<form method="post">` si nu ca un link `<a href="/Auth/Logout">`?

Logout-ul este implementat cu POST deoarece aceasta operatie modifica starea aplicatiei (utilizatorul este delogat). Conform regulilor HTTP, cererile GET nu ar trebui sa modifice starea.
Daca logout-ul ar fi un GET accesibil prin link, ar putea aparea probleme de securitate. De exemplu, un site extern ar putea forta utilizatorul sa acceseze acel link (CSRF attack), iar acesta ar fi delogat fara sa vrea. De asemenea, un simplu click accidental sau un refresh ar putea declansa logout-ul.
Prin folosirea metodei POST impreuna cu antiforgery token, aplicatia este protejata impotriva acestor atacuri.

---
2. De ce login-ul face doi pasi in loc de unul?

var user = await _userManager.FindByEmailAsync(model.Email);
var result = await _signInManager.PasswordSignInAsync(user.UserName!, ...);

Login-ul se face in doi pasi deoarece ASP.NET Core Identity foloseste UserName ca identificator principal pentru autentificare, nu Email.
Primul pas cauta utilizatorul dupa email, iar al doilea pas face autentificarea folosind UserName si parola.
Nu exista un apel direct de tipul PasswordSignInAsync(email, password) deoarece Email este doar o proprietate a utilizatorului si nu este garantat ca este identificatorul principal. UserName este cel folosit intern de Identity pentru login.

3. De ce nu este suficient sa ascunzi butoanele Edit/Delete in View?

1. Ascunderea butoanelor in View folosind conditii precum:
@if (User.Identity.IsAuthenticated)
este doar o masura de interfata (UI), nu o metoda reala de securitate.
Un utilizator poate accesa direct URL-ul unei actiuni, de exemplu:
/Articles/Delete/5 chiar daca butonul nu este vizibil in pagina.

De aceea, este necesar sa folosim si protectie in controller, prin:
[Authorize]
verificari suplimentare (ex: IsOwnerOrAdmin())
Acestea asigura securitatea reala la nivel de backend.
Daca am avea doar [Authorize] in controller, dar nu am ascunde butoanele in View, utilizatorul ar vedea optiunile dar ar primi eroare (403) la acces. Acest lucru nu afecteaza securitatea, dar duce la o experienta slaba pentru utilizator.

4. Ce este middleware pipeline-ul in ASP.NET Core?

Middleware pipeline-ul reprezinta lantul de componente prin care trece fiecare request in aplicatie.
Un request tipic trece prin mai multe etape, cum ar fi:
Request -> Authentication -> Authorization -> Controller -> Response
Fiecare middleware poate prelucra request-ul si poate decide daca il trimite mai departe.
Ordinea middleware-urilor este foarte importanta. De exemplu:

app.UseAuthentication();
app.UseAuthorization();

UseAuthentication trebuie sa fie apelat inainte de UseAuthorization deoarece:

Authentication identifica utilizatorul (cine este)
Authorization verifica daca utilizatorul are dreptul sa acceseze resursa

Daca le-am inversa, Authorization nu ar sti cine este utilizatorul si ar bloca accesul.

5. Ce am fi trebuit sa implementam manual daca nu foloseam ASP.NET Core Identity?

Fara Identity, ar fi trebuit sa implementam manual un sistem complet de autentificare si securitate, inclusiv:

- gestionarea utilizatorilor (tabel Users)
- stocarea parolelor in mod securizat (hash + salt)
- verificarea parolelor la login
- gestionarea rolurilor si a permisiunilor
- implementarea sesiunilor si a cookie-urilor
- mecanisme de logout
- protectie CSRF
- protectie impotriva atacurilor brute-force
- resetare parola si confirmare email
- optional autentificare in doi pasi (2FA)

Practic, ar fi fost nevoie sa construim de la zero un sistem complex de securitate.

6. Care sunt dezavantajele folosirii ASP.NET Core Identity?

Desi ASP.NET Core Identity este foarte util, are si cateva dezavantaje:

- este relativ complex si greu de inteles la inceput
- genereaza multe tabele in baza de date (AspNetUsers, AspNetRoles, etc.)
- este strans legat de Entity Framework Core
- este mai greu de adaptat pentru aplicatii moderne de tip SPA (React, Angular) sau aplicatii mobile
- pentru API-uri este adesea nevoie de alternative (ex: JWT)
- migrarea catre un alt sistem de autentificare poate fi dificila

Cu toate acestea, Identity economiseste foarte mult timp si ofera un sistem sigur deja testat.