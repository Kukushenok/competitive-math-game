@echo off
setlocal enabledelayedexpansion

REM Сохраняем текущую директорию
set "MAIN_DIR=%CD%"

REM Переходим в папку с батниками
cd Launchers || (
    echo Ошибка: папка Launchers не найдена!
    exit /b 1
)

REM Сохраняем путь к папке Launchers
set "LAUNCHERS_DIR=%CD%"

REM Цикл по всем батникам в папке
for %%f in (*.bat) do (
    echo.
    echo ========================================
    echo Running %%f
    echo ========================================

    REM Возвращаемся в папку Launchers перед каждым запуском
    cd /d "%LAUNCHERS_DIR%"

    call "%%f"

    echo Wait for K6...
    call :WAIT_FOR_K6

    echo Await 120 seconds...
    timeout /t 120 /nobreak >nul

    echo Killing...

    REM Удаляем k6 контейнеры
    for /f "tokens=*" %%i in ('docker ps -aq --filter "name=k6" 2^>nul') do (
        echo Killed: %%i
        docker rm -f %%i >nul 2>&1
    )

    REM Останавливаем основные сервисы docker-compose
    REM Возвращаемся в основную директорию для docker-compose
    cd /d "%MAIN_DIR%"
    docker compose -f "docker-compose.yml" -f "docker-compose.k6.yml" down --remove-orphans

    REM Возвращаемся в папку Launchers для продолжения цикла
    cd /d "%LAUNCHERS_DIR%"

    REM Удаляем сети k6
    for /f "tokens=*" %%i in ('docker network ls --filter "name=k6" -q 2^>nul') do (
        docker network rm %%i >nul 2>&1
    )

    echo IM DONE WITH %%f
    echo.
)

echo Everything is done!
pause
exit /b

:WAIT_FOR_K6
REM Проверяем, есть ли работающие контейнеры k6
docker ps --format "{{.Names}}" | findstr /i "k6" >nul
if !errorlevel! == 0 (
    REM echo k6 is still alive, waiting...
    timeout /t 10 /nobreak >nul
    goto WAIT_FOR_K6
)
echo k6 was killed.
goto :eof