powershell -nop -c "& {sleep 500}"
:again
if not exist marker.txt goto exit
dotnet test --filter Category=Measurement
goto again
:exit
echo "Stopped!"