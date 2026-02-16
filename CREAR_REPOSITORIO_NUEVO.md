# 📘 Guía: Crear Repositorio Separado para WindowTabsFree-Net8

## Objetivo

Esta guía te ayudará a crear un **repositorio Git completamente separado** para WindowTabsFree-Net8, permitiéndote trabajar de forma independiente sin afectar el código original.

---

## ¿Por Qué Crear un Repositorio Separado?

**4 Razones Principales:**

1. **Independencia Total** - Tu propio repo, tus propias reglas
2. **Historial Limpio** - Sin commits legacy de F# o WinForms
3. **Fácil de Compartir** - Solo clonar tu repo, sin código legacy
4. **Colaboración Simple** - Issues, PRs, y releases enfocados en .NET 8

---

## Opción A: Desde Cero (Recomendada)

### ✅ Ventajas
- Repositorio completamente limpio
- Sin historial legacy
- Más rápido y simple (5 minutos)
- Recomendado para la mayoría

### ❌ Desventajas
- Pierde historial de commits previos
- Empieza desde commit inicial

### Pasos

**Paso 1: Copiar el Código**
```bash
# Copia el directorio completo a un nuevo lugar
cp -r WindowTabsFree-Net8/ ~/WindowTabsFree-Net8-Standalone

# Navega al nuevo directorio
cd ~/WindowTabsFree-Net8-Standalone
```

**Paso 2: Inicializar Git**
```bash
# Inicializa un nuevo repositorio Git
git init

# Agrega todos los archivos
git add .

# Crea el commit inicial
git commit -m "Initial commit: WindowTabsFree .NET 8 implementation

- Cross-platform window management (Windows/Linux/macOS)
- Per-application auto-grouping
- Floating window manager
- Tab visualization with active highlighting
- Complete documentation (21 files)"
```

**Paso 3: Crear Repositorio en GitHub** (ver sección detallada abajo)

**Paso 4: Conectar y Push**
```bash
# Agrega el remote (reemplaza con tu URL)
git remote add origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git

# Renombra branch a main (si es necesario)
git branch -M main

# Push inicial
git push -u origin main
```

**Paso 5: Verificar**
```bash
# Verifica que build funciona
dotnet build WindowTabsFreeNet.sln

# Debería ver: Build succeeded
```

---

## Opción B: Con Historial Filtrado (Avanzada)

### ✅ Ventajas
- Mantiene historial de commits
- Útil para auditoría
- Muestra evolución del código

### ❌ Desventajas
- Más complejo
- Requiere conocimiento Git avanzado
- Repo más grande
- Toma más tiempo

### Pasos

**Paso 1: Clonar el Repositorio Original**
```bash
# Clona en un directorio temporal
git clone https://github.com/xtremevice/WindowTabsFree.git windowtabs-temp
cd windowtabs-temp
```

**Paso 2: Filtrar Solo WindowTabsFree-Net8**
```bash
# Filtra para mantener solo el subdirectorio WindowTabsFree-Net8
# ADVERTENCIA: Esto reescribe el historial
git filter-branch --subdirectory-filter WindowTabsFree-Net8 -- --all

# Ahora el subdirectorio WindowTabsFree-Net8 es la raíz
```

**Paso 3: Push a Nuevo Repositorio**
```bash
# Crea el repo en GitHub primero (ver sección abajo)

# Cambia el remote al nuevo repo
git remote set-url origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git

# Push
git push -u origin main
```

---

## Crear Repositorio en GitHub

### Paso a Paso Visual

**1. Ve a GitHub.com**
- Inicia sesión en tu cuenta
- Click en el botón "+" (arriba derecha)
- Selecciona "New repository"

**2. Configuración del Repositorio**
```
Repository name: WindowTabsFree-Net8
Description: Cross-platform window management for .NET 8 (Windows/Linux/macOS)
Visibility: ☐ Public  ☑ Private  (tu elección)

☐ Add a README file        (NO marques, ya tienes README)
☐ Add .gitignore           (NO marques, ya tienes .gitignore)
☐ Choose a license         (opcional, puedes agregar después)
```

**3. Click "Create repository"**

**4. Copia la URL del Repo**
```
https://github.com/TU_USUARIO/WindowTabsFree-Net8.git
```

**5. Usa esta URL en los comandos anteriores**

---

## Crear Repositorio en GitLab

```bash
# Similar a GitHub, pero con CLI:
# 1. Crea el proyecto en GitLab web interface

# 2. Usa la URL de GitLab:
git remote add origin https://gitlab.com/TU_USUARIO/WindowTabsFree-Net8.git
git push -u origin main
```

---

## Crear Repositorio en Bitbucket

```bash
# 1. Crea repo en Bitbucket web

# 2. Usa URL de Bitbucket:
git remote add origin https://TU_USUARIO@bitbucket.org/TU_USUARIO/windowtabsfree-net8.git
git push -u origin main
```

---

## Estructura Recomendada del Nuevo Repo

Puedes reorganizar ligeramente si quieres:

```
WindowTabsFree-Net8/  (raíz del repo)
├── .github/
│   └── workflows/
│       ├── build-linux.yml
│       ├── build-macos.yml
│       └── build-windows.yml
├── docs/
│   ├── QUICKSTART.md
│   ├── QUICKSTART_ES.md
│   ├── GUIA_AGRUPACION.md
│   └── ... (todos los .md excepto README)
├── src/
│   ├── WindowTabsFree.Common/
│   ├── WindowTabsFree.Core/
│   ├── WindowTabsFree.Services/
│   ├── WindowTabsFree.UI/
│   └── WindowTabsFree.TestConsole/
├── scripts/
│   ├── publish-linux.sh
│   ├── publish-macos-arm64.sh
│   └── check-macos-requirements.sh
├── .gitignore
├── LICENSE
├── README.md
└── WindowTabsFreeNet.sln
```

---

## Configuración Post-Creación

### 1. Actualizar README.md

Edita el README para reflejar el nuevo repo:

```markdown
# WindowTabsFree .NET 8

Cross-platform window management for Windows, Linux, and macOS.

## Installation

\`\`\`bash
git clone https://github.com/TU_USUARIO/WindowTabsFree-Net8.git
cd WindowTabsFree-Net8
dotnet build
\`\`\`

## Quick Start

\`\`\`bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
\`\`\`

For detailed instructions, see [QUICKSTART.md](QUICKSTART.md) or [QUICKSTART_ES.md](QUICKSTART_ES.md).
```

### 2. Configurar CI/CD (GitHub Actions)

Crea `.github/workflows/build.yml`:

```yaml
name: Build

on: [push, pull_request]

jobs:
  build:
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, macos-latest, windows-latest]
    
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    - name: Restore
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

### 3. Branch Protection

En GitHub:
- Settings → Branches → Add rule
- Branch name pattern: `main`
- ☑ Require pull request reviews
- ☑ Require status checks to pass

### 4. Agregar Colaboradores

En GitHub:
- Settings → Collaborators
- Add people

### 5. Configurar Releases

En GitHub:
- Releases → Create a new release
- Tag: v1.0.0
- Title: WindowTabsFree .NET 8 - First Release
- Description: (lista de features)

### 6. Agregar Badges al README

```markdown
![Build Status](https://github.com/TU_USUARIO/WindowTabsFree-Net8/workflows/Build/badge.svg)
![License](https://img.shields.io/github/license/TU_USUARIO/WindowTabsFree-Net8)
![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)
```

### 7. Configurar Issues Templates

Crea `.github/ISSUE_TEMPLATE/bug_report.md` y `feature_request.md`

---

## Verificación

Verifica que todo funciona:

```bash
# 1. Clone de prueba
git clone https://github.com/TU_USUARIO/WindowTabsFree-Net8.git test-clone
cd test-clone

# 2. Build
dotnet build WindowTabsFreeNet.sln

# 3. Run
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj

# Si todo funciona → ✅ Éxito!
```

---

## Mantenimiento de Ambos Repositorios

### Repositorio Original (WindowTabsFree)
- Contiene: Legacy code + .NET 8
- Mantenido por: xtremevice
- Uso: Referencia histórica

### Tu Nuevo Repositorio (WindowTabsFree-Net8)
- Contiene: Solo .NET 8
- Mantenido por: Tú
- Uso: Desarrollo activo

### Sin Sincronización Automática

Los dos repos son **completamente independientes**:
- Cambios en uno NO afectan al otro
- No hay sincronización automática
- Puedes divergir completamente

### Si Quieres Traer Cambios del Original (Manual)

```bash
# 1. Agregar remote al original
git remote add upstream https://github.com/xtremevice/WindowTabsFree.git

# 2. Fetch cambios
git fetch upstream

# 3. Cherry-pick commits específicos
git cherry-pick <commit-hash>

# O merge todo (no recomendado)
git merge upstream/main
```

---

## Ventajas del Repositorio Separado

### 1. **Historial Limpio**
- Solo commits de .NET 8
- Sin commits de F# o WinForms legacy
- Fácil de entender la evolución

### 2. **Sin Código Legacy**
- Solo 5 proyectos .NET 8
- Sin directorios F# (WtProgram, WtDesktop, etc.)
- Más fácil de navegar

### 3. **Más Fácil de Navegar**
- Estructura simple y clara
- Sin confusión con código legacy
- Onboarding rápido para nuevos colaboradores

### 4. **CI/CD Enfocado**
- Solo builds .NET 8
- Sin necesidad de F# tooling
- Pipelines más rápidos

### 5. **Releases Independientes**
- Tu propio versionado (v1.0.0, v2.0.0, etc.)
- Sin depender de releases del original
- Control total del roadmap

### 6. **Clone Más Rápido**
- Repo más pequeño (~10-20 MB vs ~100+ MB)
- Sin historial grande
- Downloads más rápidos

### 7. **Mejor para Colaboradores**
- Solo necesitan .NET 8
- No necesitan entender legacy code
- Setup más simple

### 8. **Issues Enfocados**
- Solo issues de .NET 8
- Sin mezclar con legacy
- Mejor organización

### 9. **Roadmap Independiente**
- Decides tus propias features
- Sin restricciones del original
- Evoluciona a tu ritmo

---

## Troubleshooting

### Error: Permission denied (publickey)

```bash
# Solución: Configura SSH key o usa HTTPS con token
git remote set-url origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git
```

### Error: Build falla después de copiar

```bash
# Verifica que tienes .NET 8 SDK
dotnet --version

# Debería mostrar 8.0.x
# Si no, instala desde: https://dotnet.microsoft.com/download/dotnet/8.0
```

### Error: Git history muy grande

```bash
# Usa Opción A en lugar de B
# O limpia el historial:
git gc --aggressive --prune=now
```

### Quiero cambiar de Opción B a Opción A

```bash
# Elimina el repo y empieza de nuevo con Opción A
cd ..
rm -rf windowtabs-temp
# Sigue Opción A desde el inicio
```

### Error: Failed to push

```bash
# Verifica que creaste el repo en GitHub primero
# Verifica que el URL del remote es correcto
git remote -v

# Si es incorrecto, cámbialo:
git remote set-url origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git
```

### Error: File too large

```bash
# Verifica que .gitignore está correcto
# No deberías commitear bin/, obj/, etc.

# Si ya lo hiciste:
git rm --cached -r bin/ obj/
git commit -m "Remove build artifacts"
```

### Error: Push rejected

```bash
# Pull primero
git pull origin main --rebase

# Resuelve conflictos si hay
# Luego push
git push origin main
```

### Error: Merge conflicts

```bash
# Abre los archivos con conflictos
# Busca marcadores: <<<<<<<, =======, >>>>>>>
# Resuelve manualmente
# Luego:
git add .
git commit -m "Resolve conflicts"
```

---

## Preguntas Frecuentes (FAQ)

### 1. ¿Pierdo el historial de commits si uso Opción A?
Sí, empiezas con un repo limpio. Si necesitas el historial, usa Opción B.

### 2. ¿Puedo sincronizar cambios entre ambos repos?
Manualmente sí (cherry-pick o merge), pero automáticamente no. No es recomendado mantener sincronización.

### 3. ¿Qué URL debo usar en la documentación?
Tu nuevo repositorio: `https://github.com/TU_USUARIO/WindowTabsFree-Net8.git`

### 4. ¿Cómo colaboran otros desarrolladores?
Clonan tu nuevo repositorio, no el original. Hacen PRs a tu repo.

### 5. ¿Afecta al repositorio original?
No, son completamente independientes. No hay conexión.

### 6. ¿Puedo hacer merge de cambios del original?
Técnicamente sí, pero no es recomendado. Los repos divergen naturalmente.

### 7. ¿Qué pasa con los issues del repo original?
Crea nuevos issues en tu repo. Los issues del original no se transfieren.

### 8. ¿Necesito permisos especiales?
Solo necesitas poder crear repositorios en GitHub/GitLab/etc. (cuenta gratuita funciona).

### 9. ¿Puedo usar otros servicios Git además de GitHub?
Sí, funciona con GitLab, Bitbucket, Gitea, o cualquier Git hosting.

### 10. ¿Cómo actualizo las dependencias?
Normal: `dotnet restore` y `dotnet build`. El package.json/csproj no cambia.

---

## Checklist Final

Antes de considerar completo:

```
□ Nuevo repositorio creado en GitHub/GitLab/Bitbucket
□ Código copiado correctamente a nueva ubicación
□ Git inicializado en el nuevo directorio
□ Commit inicial realizado con mensaje descriptivo
□ Push exitoso al remote
□ Build funciona en el nuevo repo (dotnet build)
□ README.md actualizado con nuevo URL
□ Colaboradores agregados (si aplica)
□ CI/CD configurado (opcional pero recomendado)
□ Repositorio original sin cambios (verificado)
```

---

## Ejemplo Completo (GitHub)

```bash
# === INICIO ===

# 1. Copiar código
cp -r WindowTabsFree-Net8/ ~/WindowTabsFree-Net8-Standalone
cd ~/WindowTabsFree-Net8-Standalone

# 2. Inicializar Git
git init
git add .
git commit -m "Initial commit: WindowTabsFree .NET 8

- Cross-platform (Windows/Linux/macOS)
- Auto-grouping per application
- Floating window manager
- Complete documentation"

# 3. Crear repo en GitHub
# (Ir a github.com, New Repository, copiar URL)

# 4. Push
git remote add origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git
git branch -M main
git push -u origin main

# 5. Verificar
dotnet build WindowTabsFreeNet.sln

# === ✅ COMPLETO ===
```

---

## Resumen

Has aprendido:
- ✅ Dos formas de crear repo separado (simple y avanzada)
- ✅ Comandos exactos para cada plataforma
- ✅ Cómo configurar el nuevo repo
- ✅ Cómo mantener independencia
- ✅ Troubleshooting para problemas comunes

**Tiempo estimado:** 5-10 minutos

**Resultado:** Repositorio completamente independiente, listo para desarrollo sin afectar el código original.

---

## Próximos Pasos

Después de crear tu repo:

1. **Desarrolla nuevas features** - Es tu repo, tu decides
2. **Configura CI/CD** - Automated builds y tests
3. **Invita colaboradores** - Trabaja en equipo
4. **Crea releases** - Versionado independiente
5. **Comparte** - Tu repo es fácil de compartir

¡Buena suerte con tu nuevo repositorio independiente! 🚀
