# Project snapshot for AI agents — tvp (short)

## Quick high-level summary ✅
- This repo contains a **legacy ASP.NET Web Forms site** (many `.aspx` + `.aspx.cs` pages) targeted at **.NET Framework 4.x** and a **modern ASP.NET Core MVC project** at `ETCTS/` (TargetFramework `net7.0`).
- Common runtime/deploy artifacts live in `Bin/` (3rd-party UI controls, Azure storage libs, iTextSharp, NewtonSoft.Json, AjaxControlToolkit, obout controls). Keep them in place for local runs.

---

## What an agent should know first (why it is structured this way) 💡
- The WebForms site is organized as page-per-feature (UI + code-behind). Business logic is primarily inside page code-behind files rather than a layered service architecture.
- The `ETCTS` project is a separate, newer MVC app (Controllers/Views/Models). It does not appear tightly coupled to the legacy pages — treat it as an independent app unless asked otherwise.

---

## Developer workflows & how to run things locally 🔧
- ETCTS (modern app)
  - Build: `dotnet build ETCTS/ETCTS.csproj`
  - Run: `dotnet run --project ETCTS` (or `dotnet watch run` for iterative dev)
- Legacy WebForms site (classic)
  - Open the folder in **Visual Studio (VS 2019/2022)** as a *Web Site* or convert it to a Web Application to get full compile-time checks.
  - Debug: F5 in Visual Studio (IIS Express) or attach to `w3wp` for full IIS.
  - CI/automatic builds: there isn't a `.csproj` for the website; `msbuild tvp.sln` may not compile the website automatically — prefer building ETCTS directly and use Visual Studio to precompile the WebForms site or add a Web Application Project.

---

## App-specific patterns & conventions (examples) 🔎
- DB connection names (used throughout): **`salespipeline`** and **`salespipeline2`** (see `web.config` and many pages like `request.aspx`, `TestCustomer.aspx`).
  - Example (pattern):
    - `SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString);`
    - Several pages use `SqlDataSource` controls in markup (`<asp:SqlDataSource ... ConnectionString="<%$ ConnectionStrings:salespipeline %>" />`).
- Email: SMTP is used directly via `System.Net.Mail.SmtpClient` (many pages instantiate `new SmtpClient("localhost")` for dev; `web.config` contains SMTP settings to override for deployed environments).
- UI: pages rely on 3rd-party controls (AjaxControlToolkit register directives in many `.aspx` files). Ensure `Bin/` contains required DLLs when running locally.
- Naming: page prefixes map to domain areas (e.g., `cluAgent*` = agent cluster pages, `pref*` = preferences options, `minfo*` = manager info, `input*` = forms).

---

## Integration points & external dependencies 🔗
- Database(s): referenced by `web.config` connection strings (local dev must supply working connections; tables like `Customers`, `StatusDDL` are used by pages).
- SMTP/email: configured in `web.config` but code often uses `localhost` for dev; set correctly in dev/staging as needed.
- 3rd-party UI/utility libraries: `AjaxControlToolkit`, `iTextSharp`, `HtmlAgilityPack`, `Newtonsoft.Json`, `Microsoft.WindowsAzure.Storage` (DLLs live in `Bin/`).

---

## Code practices to follow for this repo 🧭
- Keep changes minimal and targeted: many pages tightly couple UI+DB; prefer fixing or adding behavior in the page’s code-behind to avoid large refactors without a clear migration plan.
- When modifying data access, keep the established ADO.NET/SqlCommand style consistent and ensure parameterized queries are used (many queries already use parameters—follow those patterns).
- Do not remove or replace `Bin/` assemblies without verifying dependent pages and testing UI pages locally.

---

## Existing docs & gaps 📚
- There is no repo-level README or `.github/copilot-instructions.md` yet (this file is newly added).
- There appear to be no automated unit tests; plan manual verification steps and local testing instructions in PRs.

---

## Common places to look for changes / samples 👀
- DB and ops usage: `request.aspx` / `request.aspx.cs`, `TestCustomer.aspx.cs`, `Test2Customer.aspx.cs` — good examples of DB insert/update and email send flows.
- Ajax/UI patterns: `MasterPage.master`, `Site.master`, `Admin/` folder pages and master pages for admin UI.
- Modern code sample: `ETCTS/Controllers/HomeController.cs` and `ETCTS/Models/` show idiomatic ASP.NET Core structure if adding new APIs or services.

---

## Safety notes & quick checks ⚠️
- Some configuration placeholders/credentials appear in `web.config` (commented or present). Treat them as secrets; do not commit sensitive live credentials into source control.
- Because data access is raw SQL in many places, watch for accidental SQL injection when altering query strings.

---

If anything here is unclear or you'd like the instructions tailored (e.g., add CI steps, or more detailed debugging recipes), tell me which area to expand and I’ll iterate. ✅
