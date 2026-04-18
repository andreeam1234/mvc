### 1. De ce Logout este implementat ca `<form method="post">` si nu ca un link `<a href="/Auth/Logout">`?

Logout-ul este implementat cu POST deoarece aceasta actiune modifica starea aplicatiei (utilizatorul este delogat). Conform regulilor HTTP, cererile GET trebuie folosite doar pentru citire de date, nu pentru modificari.

Daca logout-ul ar fi un GET accesibil prin link, ar putea aparea probleme de securitate. De exemplu, un alt site ar putea forta accesarea acelui link (CSRF attack), iar utilizatorul ar fi delogat fara sa isi dea seama.

Prin folosirea metodei POST impreuna cu antiforgery token, aplicatia este protejata impotriva acestor atacuri.

### 2. De ce login-ul face doi pasi in loc de unul?

```csharp
var user = await _userManager.FindByEmailAsync(model.Email);
var result = await _signInManager.PasswordSignInAsync(user.UserName!, ...);
```
Login-ul se face in doi pasi deoarece ASP.NET Core Identity foloseste UserName ca identificator principal pentru autentificare, nu Email.

In primul pas se cauta utilizatorul dupa email. In al doilea pas, autentificarea se face folosind UserName si parola.

Nu exista un apel direct de tipul PasswordSignInAsync(email, password) deoarece email-ul este doar o proprietate a utilizatorului si nu este identificatorul principal in sistem.

### 3. De ce nu este suficient sa ascunzi butoanele Edit/Delete in View?

Ascunderea butoanelor in View este doar o masura de interfata, nu o metoda reala de securitate.

Un utilizator poate accesa direct un URL de tip: /Articles/Delete/5

chiar daca butonul nu este vizibil in pagina.

De aceea, securitatea trebuie implementata in controller folosind:
- `[Authorize]`
- verificari suplimentare (ex: daca utilizatorul este autorul sau admin)

Daca am avea doar verificari in controller, dar nu am ascunde butoanele in View, utilizatorul ar vedea optiunile dar ar primi eroare (403), ceea ce afecteaza experienta utilizatorului.
### 4. Ce este middleware pipeline-ul in ASP.NET Core?

Middleware pipeline-ul reprezinta lantul de componente prin care trece fiecare request in aplicatie, de la primire pana la raspuns.

Un exemplu simplu: Request -> Authentication -> Authorization -> Controller -> Response

Ordinea middleware-urilor este importanta:
```csharp
app.UseAuthentication();
app.UseAuthorization();
```
UseAuthentication trebuie sa fie apelat inainte de UseAuthorization deoarece:

Authentication identifica utilizatorul
Authorization verifica daca utilizatorul are drepturi

Daca le inversam, aplicatia nu va sti cine este utilizatorul si va bloca accesul.

### 5. Ce am fi trebuit sa implementam manual daca nu foloseam ASP.NET Core Identity?

Fara Identity, ar fi trebuit sa implementam manual un sistem complet de autentificare si securitate:

- gestionarea utilizatorilor
- stocarea parolelor in mod securizat (hash + salt)
- verificarea parolelor
- sistem de login si logout
- gestionarea sesiunilor si cookie-urilor
- roluri si permisiuni
- protectie CSRF
- protectie impotriva atacurilor brute-force
- resetare parola si confirmare email

Practic, ar fi fost nevoie sa construim de la zero un sistem complex de securitate.
### 6. Care sunt dezavantajele folosirii ASP.NET Core Identity?

Desi este util, ASP.NET Core Identity are si dezavantaje:

- este destul de complex si greu de inteles la inceput
- creeaza multe tabele in baza de date
- este dependent de Entity Framework Core
- este mai greu de folosit in aplicatii moderne (React, Angular, mobile)
- pentru API-uri este nevoie de solutii alternative (ex: JWT)
- migrarea catre alt sistem poate fi dificila

Totusi, avantajul principal este ca ofera un sistem de autentificare sigur si complet, economisind mult timp de dezvoltare.
