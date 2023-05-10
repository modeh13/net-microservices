# Build Images
$dockerFiles = @{ 
    "shopping-cart" = ".\shopping-cart";
    "special-offers" = ".\special-offers";
    "loyalty-program" = ".\loyalty-program";
};

ForEach ($dockerFile in $dockerFiles.keys)
{
    Write-Output "Building Docker Image $($dockerFile) using $($dockerFiles[$dockerFile]) Dockerfile";
    docker build $dockerFiles[$dockerFile] -t $dockerFile;
}
