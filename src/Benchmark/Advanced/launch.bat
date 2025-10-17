@echo off
setlocal enabledelayedexpansion

REM Переходим в папку с батниками
cd Launchers || (
    echo Ошибка: папка Launchers не найдена!
    exit /b 1
)

REM Цикл по всем батникам в папке
for %%f in (*.bat) do (
    echo.
    echo ========================================
    echo Running %%f
    echo ========================================

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
    docker compose -f "%CD%\..\..\..\docker-compose.yml" -f "%CD%\..\..\..\docker-compose.k6.yml" down --remove-orphans

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
