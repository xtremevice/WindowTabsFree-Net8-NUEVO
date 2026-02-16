#!/bin/bash

# WindowTabsFree - macOS Requirements Checker
# Verifica que tienes todo lo necesario para ejecutar en Mac

echo "======================================"
echo "WindowTabsFree - Verificador de Requisitos para macOS"
echo "======================================"
echo ""

# Colores
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

ERRORS=0
WARNINGS=0

echo -e "${BLUE}🔍 Verificando requisitos del sistema...${NC}"
echo ""

# 1. Verificar macOS
echo -n "1. Sistema Operativo: "
if [[ "$OSTYPE" == "darwin"* ]]; then
    VERSION=$(sw_vers -productVersion)
    echo -e "${GREEN}✅ macOS $VERSION${NC}"
    
    # Verificar versión mínima (10.15)
    MAJOR=$(echo $VERSION | cut -d. -f1)
    MINOR=$(echo $VERSION | cut -d. -f2)
    if [ "$MAJOR" -lt 10 ] || ([ "$MAJOR" -eq 10 ] && [ "$MINOR" -lt 15 ]); then
        echo -e "   ${RED}⚠️  Advertencia: Se requiere macOS 10.15 o superior${NC}"
        WARNINGS=$((WARNINGS + 1))
    fi
else
    echo -e "${RED}❌ No es macOS${NC}"
    ERRORS=$((ERRORS + 1))
fi

# 2. Verificar arquitectura
echo -n "2. Arquitectura: "
ARCH=$(uname -m)
if [ "$ARCH" == "arm64" ]; then
    echo -e "${GREEN}✅ Apple Silicon (ARM64)${NC}"
    echo -e "   ${BLUE}ℹ️  Usa: publish-macos-arm64.sh para compilar${NC}"
elif [ "$ARCH" == "x86_64" ]; then
    echo -e "${YELLOW}⚠️  Intel (x86_64)${NC}"
    echo -e "   ${BLUE}ℹ️  Usa runtime: osx-x64 para compilar${NC}"
    WARNINGS=$((WARNINGS + 1))
else
    echo -e "${RED}❌ Arquitectura desconocida: $ARCH${NC}"
    ERRORS=$((ERRORS + 1))
fi

# 3. Verificar .NET SDK
echo -n "3. .NET SDK: "
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    DOTNET_MAJOR=$(echo $DOTNET_VERSION | cut -d. -f1)
    
    if [ "$DOTNET_MAJOR" -ge 8 ]; then
        echo -e "${GREEN}✅ .NET $DOTNET_VERSION${NC}"
    else
        echo -e "${YELLOW}⚠️  .NET $DOTNET_VERSION (se requiere 8.0 o superior)${NC}"
        WARNINGS=$((WARNINGS + 1))
    fi
    
    # Mostrar runtimes instalados
    echo -e "   ${BLUE}ℹ️  Runtimes instalados:${NC}"
    dotnet --list-runtimes | grep "Microsoft.NETCore.App 8" | sed 's/^/      /'
else
    echo -e "${RED}❌ No instalado${NC}"
    echo -e "   ${YELLOW}Instalar desde: https://dotnet.microsoft.com/download${NC}"
    ERRORS=$((ERRORS + 1))
fi

# 4. Verificar Git
echo -n "4. Git: "
if command -v git &> /dev/null; then
    GIT_VERSION=$(git --version | cut -d' ' -f3)
    echo -e "${GREEN}✅ Git $GIT_VERSION${NC}"
else
    echo -e "${RED}❌ No instalado${NC}"
    echo -e "   ${YELLOW}Instalar: xcode-select --install${NC}"
    ERRORS=$((ERRORS + 1))
fi

# 5. Verificar ubicación del proyecto
echo -n "5. Proyecto WindowTabsFree: "
if [ -f "WindowTabsFreeNet.sln" ]; then
    echo -e "${GREEN}✅ Encontrado${NC}"
    echo -e "   ${BLUE}ℹ️  Directorio: $(pwd)${NC}"
else
    echo -e "${YELLOW}⚠️  No se encontró en el directorio actual${NC}"
    echo -e "   ${BLUE}ℹ️  Buscar con: find ~ -name 'WindowTabsFree' -type d${NC}"
    WARNINGS=$((WARNINGS + 1))
fi

# 6. Verificar permisos de Xcode Command Line Tools
echo -n "6. Xcode Command Line Tools: "
if xcode-select -p &> /dev/null; then
    XCODE_PATH=$(xcode-select -p)
    echo -e "${GREEN}✅ Instalado${NC}"
    echo -e "   ${BLUE}ℹ️  Ubicación: $XCODE_PATH${NC}"
else
    echo -e "${YELLOW}⚠️  No instalado (recomendado)${NC}"
    echo -e "   ${BLUE}ℹ️  Instalar: xcode-select --install${NC}"
    WARNINGS=$((WARNINGS + 1))
fi

echo ""
echo "======================================"
echo -e "${BLUE}📊 Resumen:${NC}"

if [ $ERRORS -eq 0 ] && [ $WARNINGS -eq 0 ]; then
    echo -e "${GREEN}✅ ¡Todo listo! Tienes todos los requisitos necesarios.${NC}"
    echo ""
    echo -e "${BLUE}🚀 Próximos pasos:${NC}"
    
    if [ "$ARCH" == "arm64" ]; then
        echo "   1. Compilar: ./publish-macos-arm64.sh"
        echo "   2. Ejecutar: ./publish/macos-arm64/WindowTabsFree.UI"
    else
        echo "   1. Compilar: dotnet publish -r osx-x64 --self-contained"
        echo "   2. Ejecutar: dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj"
    fi
    
    echo ""
    echo -e "${BLUE}📚 Guías disponibles:${NC}"
    echo "   - GUIA_MAC_APPLE_SILICON.md (paso a paso en español)"
    echo "   - MACOS_GUIDE.md (guía general en inglés)"
    
elif [ $ERRORS -eq 0 ]; then
    echo -e "${YELLOW}⚠️  Hay $WARNINGS advertencia(s) pero puedes continuar.${NC}"
    echo ""
    echo -e "${BLUE}💡 Recomendación:${NC}"
    echo "   Revisa las advertencias arriba para una mejor experiencia."
else
    echo -e "${RED}❌ Hay $ERRORS error(es) crítico(s) que debes solucionar.${NC}"
    echo ""
    echo -e "${BLUE}🔧 Acción requerida:${NC}"
    echo "   1. Soluciona los errores marcados con ❌"
    echo "   2. Vuelve a ejecutar este script"
    echo ""
    echo -e "${BLUE}📚 Ayuda:${NC}"
    echo "   Ver GUIA_MAC_APPLE_SILICON.md sección 'Instalar Dependencias'"
fi

echo ""
echo -e "${BLUE}ℹ️  Información adicional:${NC}"
echo "   • Permisos de Accesibilidad se configuran DESPUÉS de ejecutar"
echo "   • La primera compilación puede tardar varios minutos"
echo "   • El ejecutable final será ~90-100MB (incluye .NET Runtime)"
echo ""

exit $ERRORS
