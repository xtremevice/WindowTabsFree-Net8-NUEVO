# Comandos para Migrar WindowTabsFree-Net8 a Repositorio Separado

## Objetivo

Esta guía proporciona los comandos EXACTOS para:
1. Descargar todos los cambios del proyecto actual
2. Descargar la última versión al repositorio WindowTabsFree-Net8
3. Pasar los cambios y proyectos a ese repositorio
4. Subir los cambios para trabajar con ese repositorio

## Requisitos Previos

- Git instalado
- Cuenta de GitHub (o GitLab/Bitbucket)
- .NET 8 SDK instalado (para verificación)

## PASO 1: Actualizar Repositorio Original

```bash
# Navegar al repositorio actual
cd /ruta/a/tu/WindowTabsFree

# Cambiar a la rama con todos los cambios
git checkout copilot/implement-window-service-functionality

# Descargar todos los cambios más recientes
git pull origin copilot/implement-window-service-functionality
```

**Verificar:**
```bash
git log -1 --oneline
# Debe mostrar el último commit
```

## PASO 2: Crear Nuevo Repositorio en GitHub

**Opción A: Por interfaz web (Recomendado)**

1. Ir a https://github.com/new
2. Repository name: `WindowTabsFree-Net8`
3. Description: `WindowTabsFree .NET 8 Cross-Platform Implementation`
4. Public o Private (tu elección)
5. **NO marcar** "Initialize this repository with a README"
6. Click "Create repository"

**Opción B: Por línea de comandos (con gh CLI)**
```bash
gh repo create WindowTabsFree-Net8 --public --description "WindowTabsFree .NET 8 Implementation"
```

## PASO 3: Copiar Todo el Contenido de WindowTabsFree-Net8

```bash
# Crear directorio para el nuevo repositorio
mkdir -p ~/Repos/WindowTabsFree-Net8

# Navegar al directorio
cd ~/Repos/WindowTabsFree-Net8

# Copiar TODO el contenido del directorio WindowTabsFree-Net8
# IMPORTANTE: Reemplaza /ruta/a/tu/WindowTabsFree con la ruta real
cp -r /ruta/a/tu/WindowTabsFree/WindowTabsFree-Net8/* .

# Copiar también el .gitignore (archivos ocultos)
cp /ruta/a/tu/WindowTabsFree/WindowTabsFree-Net8/.gitignore .
```

**Verificar:**
```bash
ls -la
# Debes ver: src/, docs/, scripts/, .gitignore, WindowTabsFreeNet.sln, etc.

# Contar archivos
ls -la | wc -l
# Debe mostrar muchos archivos (250+)
```

## PASO 4: Inicializar Git en el Nuevo Directorio

```bash
# Asegúrate de estar en el nuevo directorio
cd ~/Repos/WindowTabsFree-Net8

# Inicializar repositorio Git
git init

# Agregar todos los archivos al staging
git add .

# Verificar qué se va a commitear
git status

# Crear commit inicial
git commit -m "Initial commit: WindowTabsFree .NET 8 implementation

Complete .NET 8 cross-platform implementation with:
- Windows, Linux, and macOS support via platform-specific services
- Auto-grouping by application with per-app configuration
- Floating window manager with always-on-top capability
- Tab visualization and navigation system
- 22 comprehensive documentation files (Spanish and English)
- Build scripts for Linux and macOS
- Complete test console for validation"
```

**Verificar:**
```bash
git log -1
# Debe mostrar tu commit inicial
```

## PASO 5: Conectar con GitHub y Hacer Push

```bash
# Conectar con el repositorio remoto que creaste
# IMPORTANTE: Reemplaza TU_USUARIO con tu nombre de usuario de GitHub
git remote add origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git

# Cambiar a rama main (convención moderna)
git branch -M main

# Push inicial
git push -u origin main
```

**Si tienes error de autenticación:**
```bash
# Opción 1: Usar SSH en lugar de HTTPS
git remote set-url origin git@github.com:TU_USUARIO/WindowTabsFree-Net8.git

# Opción 2: Usar token de acceso personal (PAT)
# Crear un PAT en: https://github.com/settings/tokens
# Usar el PAT como contraseña cuando git lo pida
```

**Verificar:**
```bash
git remote -v
# Debe mostrar:
# origin  https://github.com/TU_USUARIO/WindowTabsFree-Net8.git (fetch)
# origin  https://github.com/TU_USUARIO/WindowTabsFree-Net8.git (push)

git branch -vv
# Debe mostrar: main ... [origin/main]
```

## PASO 6: Verificar que Todo Funciona

```bash
# Verificar que el proyecto compila
dotnet build WindowTabsFreeNet.sln

# Debe mostrar:
# Build succeeded.
#     0 Warning(s)
#     0 Error(s)
```

**Si tienes errores de build:**
```bash
# Verificar que tienes .NET 8 SDK
dotnet --version
# Debe ser 8.x.x

# Si no, instalar desde: https://dotnet.microsoft.com/download/dotnet/8.0
```

## PASO 7: Trabajar desde el Nuevo Repositorio

```bash
# Desde ahora, siempre trabajar en el nuevo directorio
cd ~/Repos/WindowTabsFree-Net8

# Abrir en tu editor favorito
code .  # Para VS Code
# o
rider .  # Para JetBrains Rider

# Workflow normal de desarrollo
git add .
git commit -m "Nueva feature o fix"
git push
```

---

## 🚀 Comandos de Un Solo Bloque (Copy-Paste)

Si prefieres copiar todo de una vez, aquí están los bloques unificados:

### Bloque 1: Actualizar Repositorio Original
```bash
cd /ruta/a/tu/WindowTabsFree && \
git checkout copilot/implement-window-service-functionality && \
git pull origin copilot/implement-window-service-functionality && \
echo "✓ Repositorio actualizado"
```

### Bloque 2: Copiar y Setup (después de crear repo en GitHub)
```bash
mkdir -p ~/Repos/WindowTabsFree-Net8 && \
cd ~/Repos/WindowTabsFree-Net8 && \
cp -r /ruta/a/tu/WindowTabsFree/WindowTabsFree-Net8/* . && \
cp /ruta/a/tu/WindowTabsFree/WindowTabsFree-Net8/.gitignore . && \
git init && \
git add . && \
git commit -m "Initial commit: WindowTabsFree .NET 8 implementation" && \
echo "✓ Repositorio inicializado"
```

### Bloque 3: Push (reemplaza TU_USUARIO)
```bash
git remote add origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git && \
git branch -M main && \
git push -u origin main && \
echo "✓ Código subido a GitHub"
```

---

## ✅ Verificación Completa

Después de completar todos los pasos, verifica:

```bash
# 1. Estás en el directorio correcto
pwd
# Debe mostrar: /home/usuario/Repos/WindowTabsFree-Net8

# 2. Git está configurado correctamente
git remote -v
# Debe mostrar tu repositorio en GitHub

# 3. Tienes todos los archivos
ls -la | wc -l
# Debe mostrar ~250+ archivos

# 4. El proyecto compila
dotnet build WindowTabsFreeNet.sln
# Debe compilar sin errores

# 5. Puedes ver tu repositorio en GitHub
# Ir a: https://github.com/TU_USUARIO/WindowTabsFree-Net8
```

---

## 🔧 Troubleshooting

### Error: Permission denied (publickey)

**Problema:** Git no puede autenticar con GitHub via SSH.

**Solución:**
```bash
# Cambiar a HTTPS
git remote set-url origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git
git push -u origin main
```

### Error: Repository not found

**Problema:** El repositorio no existe o la URL es incorrecta.

**Solución:**
```bash
# Verificar que creaste el repo en GitHub primero
# Verificar la URL
git remote -v

# Si es incorrecta, corregir
git remote set-url origin https://github.com/TU_USUARIO/WindowTabsFree-Net8.git
```

### Error: Failed to push some refs

**Problema:** El repositorio remoto tiene contenido que no tienes localmente.

**Solución:**
```bash
# Pull primero (si el repo no estaba vacío)
git pull origin main --allow-unrelated-histories

# Luego push
git push -u origin main
```

### Error: dotnet build falla

**Problema:** .NET 8 SDK no está instalado o hay errores en el código.

**Solución:**
```bash
# Verificar versión de .NET
dotnet --version

# Si no es 8.x, instalar desde:
# https://dotnet.microsoft.com/download/dotnet/8.0

# Limpiar y restaurar
dotnet clean
dotnet restore
dotnet build
```

---

## 📝 Notas Importantes

1. **Reemplazar rutas:** Todas las referencias a `/ruta/a/tu/WindowTabsFree` deben reemplazarse con la ruta real en tu sistema.

2. **Reemplazar usuario:** Todas las referencias a `TU_USUARIO` deben reemplazarse con tu nombre de usuario de GitHub.

3. **El repositorio original NO se modifica:** Esta secuencia de comandos solo copia el contenido. El repo original en `/ruta/a/tu/WindowTabsFree` permanece intacto.

4. **Trabajar solo en el nuevo repo:** Después de completar esta migración, todo el desarrollo debe hacerse en `~/Repos/WindowTabsFree-Net8`.

5. **Independencia total:** El nuevo repositorio es completamente independiente. No hay sincronización automática con el original.

---

## ⏱️ Tiempo Estimado

- **Paso 1** (Actualizar original): 1 minuto
- **Paso 2** (Crear repo GitHub): 2 minutos
- **Paso 3** (Copiar archivos): 1 minuto
- **Paso 4** (Git init y commit): 1 minuto
- **Paso 5** (Push): 1 minuto
- **Paso 6** (Verificar): 1 minuto

**Total: 6-7 minutos**

---

## 🎯 Resumen del Workflow

1. ✅ Actualizas el repo original para tener los últimos cambios
2. ✅ Creas un nuevo repositorio vacío en GitHub
3. ✅ Copias todo el contenido de WindowTabsFree-Net8 a un nuevo directorio
4. ✅ Inicializas Git y creas el commit inicial
5. ✅ Conectas con GitHub y haces push
6. ✅ Verificas que todo funciona (build exitoso)
7. ✅ Trabajas desde el nuevo repositorio

---

## 🎉 ¡Listo!

Ahora tienes un repositorio completamente independiente para WindowTabsFree-Net8. Puedes:

- ✅ Trabajar sin afectar el código original
- ✅ Compartir el repositorio fácilmente
- ✅ Colaborar con otros
- ✅ Hacer releases independientes
- ✅ Configurar CI/CD específico
- ✅ Tener un historial limpio

**Para empezar a trabajar:**
```bash
cd ~/Repos/WindowTabsFree-Net8
code .
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

¡A desarrollar! 🚀
