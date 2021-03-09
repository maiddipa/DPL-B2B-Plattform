##Templates taken from:
https://github.com/RicoSuter/NSwag/tree/master/src/NSwag.CodeGeneration.TypeScript/Templates

##Customizations
- When Get / Head request and request has query parameters
- Wrap all parameters into request object

##Note
- SInce all query string parameters are individual parameters in the openapi specification
- Name of complex type is not part of the openapi specification
- meaning the type that wraps the parameters is anonymous
- but it is fully typed