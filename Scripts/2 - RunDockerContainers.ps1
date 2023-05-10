# Create Docker Container if Image exists
$dockerImages = @("shopping-cart", "special-offers", "loyalty-program");
$dockerPort = 8080;

ForEach ($dockerImage in $dockerImages)
{
    $dockerImagesFilter = docker images $dockerImage;    

    if ($dockerImagesFilter.GetType().BaseType.Name -eq "Array" -and $dockerImagesFilter[1].Contains($dockerImage))
    {
        $dockerPort += 1;
        Write-Output "-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
        Write-Output "Docker Image: $($dockerImage)...";
        Write-Output "Creating Docker Container $($dockerImage) at Port $($dockerPort)...";        
        docker run -d --name $dockerImage --rm -p "$($dockerPort):80" $dockerImage;        
        Write-Output "-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
    }
}
