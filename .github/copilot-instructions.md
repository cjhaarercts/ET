# Project snapshot for AI agents — tvp (short)

## Quick high-level summary ✅
- This repo contains a **legacy ASP.NET Web Forms site** (many `.aspx` + `.aspx.cs` pages) targeted at **.NET Framework 4.0** and a **modern ASP.NET Core MVC project** at `ETCTS/` (TargetFramework `net7.0`).
- Common runtime/deploy artifacts live in `Bin/` (3rd-party UI controls like AjaxControlToolkit, obout suite; utilities like iTextSharp, HtmlAgilityPack, Newtonsoft.Json; Azure storage libs). Keep them in place for local runs.
- The WebForms site handles customer leads, agent management, and funeral home lookups for veterans' programs. ETCTS appears to be a newer, separate MVC app (possibly for modernized features), not tightly coupled to legacy pages.

---

## What an agent should know first (why it is structured this way) 💡
- The WebForms site is organized as page-per-feature (UI + code-behind). Business logic is primarily inside page code-behind files rather than a layered service architecture. This reflects a rapid-prototyping approach where each page handles its own DB queries, email sends, and UI logic.
- The `ETCTS` project is a separate, newer MVC app (Controllers/Views/Models) with standard ASP.NET Core structure. It does not appear tightly coupled to the legacy pages — treat it as an independent app unless adding integration.
- Authentication and role management have been disabled (see `web.config`: `<authentication mode="None" />`, `<roleManager enabled="false" />`), making the site public-facing for lead generation.

---

## Developer workflows & how to run things locally 🔧
- ETCTS (modern app)
  - Build: `dotnet build ETCTS/ETCTS.csproj`
  - Run: `dotnet run --project ETCTS` (or `dotnet watch run` for iterative dev)
- Legacy WebForms site (classic)
  - Open the folder in **Visual Studio (VS 2019/2022)** as a *Web Site* or convert it to a Web Application to get full compile-time checks.
  - Debug: F5 in Visual Studio (IIS Express) or attach to `w3wp` for full IIS.
  - CI/automatic builds: there isn't a `.csproj` for the website; `msbuild tvp.sln` may not compile the website automatically — prefer building ETCTS directly and use Visual Studio to precompile the WebForms site or add a Web Application Project.
- Local DB/email setup: Configure `web.config` connection strings for `salespipeline` (and optionally `salespipeline2`) DB. For email, use `localhost` SMTP in code for dev (e.g., `new SmtpClient("localhost")`), but `web.config` has network settings for prod.

---

## App-specific patterns & conventions (examples) 🔎
- DB connection names (used throughout): **`salespipeline`** and **`salespipeline2`** (see `web.config` and many pages like `request.aspx`, `TestCustomer.aspx`).
  - Example (pattern):
    - `SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString);`
    - Several pages use `SqlDataSource` controls in markup (`<asp:SqlDataSource ... ConnectionString="<%$ ConnectionStrings:salespipeline %>" />`).
  - Key tables: `Customers` (for leads: FirstName, LastName, EmailAddress, etc.), `StatusDDL` (dropdown values).
  - Queries: Raw SQL with parameterized commands (e.g., `cmd.Parameters.AddWithValue("@FirstName", FirstName.Text);`). Follow this for inserts/updates to avoid injection.
- Email: SMTP is used directly via `System.Net.Mail.SmtpClient` (many pages instantiate `new SmtpClient("localhost")` for dev; `web.config` contains SMTP settings to override for deployed environments).
  - Example: In `request.aspx.cs`, send lead notification emails with hardcoded recipients.
- UI: pages rely on 3rd-party controls (AjaxControlToolkit register directives in many `.aspx` files, e.g., `<ajaxToolkit:ModalPopupExtender>`; obout suite for calendars/menus). GridViews with row highlighting and modal edits are common (see `TestCustomer.aspx.cs`).
- Naming: page prefixes map to domain areas (e.g., `cluAgent*` = agent cluster pages, `pref*` = preferences options, `minfo*` = manager info, `input*` = forms, `custlookup*` = customer lookups).

---

## Integration points & external dependencies 🔗
- Database(s): referenced by `web.config` connection strings (local dev must supply working connections; tables like `Customers`, `StatusDDL` are used by pages).
- SMTP/email: configured in `web.config` but code often uses `localhost` for dev; set correctly in dev/staging as needed.
- 3rd-party UI/utility libraries: `AjaxControlToolkit`, `obout_*` (calendars, menus), `iTextSharp` (PDF generation), `HtmlAgilityPack` (HTML parsing), `Newtonsoft.Json` (JSON handling), `Microsoft.WindowsAzure.Storage` (cloud storage). DLLs live in `Bin/`.
- No external APIs apparent; all integrations are DB and email.

---

## Code practices to follow for this repo 🧭
- Keep changes minimal and targeted: many pages tightly couple UI+DB; prefer fixing or adding behavior in the page's code-behind to avoid large refactors without a clear migration plan.
- When modifying data access, keep the established ADO.NET/SqlCommand style consistent and ensure parameterized queries are used (many queries already use parameters—follow those patterns, e.g., `AddWithValue`).
- For UI updates, reuse existing controls like GridView with AjaxToolkit extenders for modals/calendars.
- Email sends: Use `SmtpClient` with localhost for dev, but respect `web.config` network settings.
- Do not remove or replace `Bin/` assemblies without verifying dependent pages and testing UI pages locally.
- No automated tests exist; manual verification required after changes.

---

## Existing docs & gaps 📚
- There is no repo-level README or `.github/copilot-instructions.md` yet (this file is newly added).
- There appear to be no automated unit tests; plan manual verification steps and local testing instructions in PRs.
- An archived version of these instructions exists in `archived_backups/backup-remove-login-pages-keep-admin-20260109/.github/copilot-instructions.md` — check for any additional historical context if needed.

---

## Common places to look for changes / samples 👀
- DB and ops usage: `request.aspx` / `request.aspx.cs`, `TestCustomer.aspx.cs`, `cluAgentActive.aspx.cs` — good examples of DB insert/update and email send flows.
- Ajax/UI patterns: `MasterPage.master`, `Site.master`, `admin/` folder pages and master pages for admin UI; `TestCustomer.aspx` for GridView/modals.
- Modern code sample: `ETCTS/Controllers/HomeController.cs` and `ETCTS/Models/` show idiomatic ASP.NET Core structure if adding new APIs or services.
- Email templates: `EmailTemplates/` folder for HTML email bodies.

---

## Safety notes & quick checks ⚠️
- Some configuration placeholders/credentials appear in `web.config` (commented or present). Treat them as secrets; do not commit sensitive live credentials into source control.
- Because data access is raw SQL in many places, watch for accidental SQL injection when altering query strings (always use parameters).
- Authentication removed: site is public; ensure any new features don't assume user context.

---

If anything here is unclear or you'd like the instructions tailored (e.g., add CI steps, or more detailed debugging recipes), tell me which area to expand and I'll iterate. ✅
