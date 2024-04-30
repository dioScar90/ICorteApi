#!/bin/bash

# Diretório onde os arquivos serão procurados e renomeados
diretorio="C:/xampp/htdocs/diogo/icorte/src"

# Encontra e renomeia os arquivos .ts para .js
find "$diretorio" -type f -name "*.ts" -exec sh -c 'mv "$1" "${1%.ts}.js"' _ {} \;

# Encontra e renomeia os arquivos .tsx para .jsx
find "$diretorio" -type f -name "*.tsx" -exec sh -c 'mv "$1" "${1%.tsx}.jsx"' _ {} \;
