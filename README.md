# TMG.Visum

VISUM support for XTMF 1 and 2

The goal of this repository is to provide the modules for XTMF 1 and 2 to support automating
VISUM allowing it to be integrated with GTAModel.

## Setup

When downloading this repository it will expect that there is a sister repository of XTMF1 at
`..\XTMF-Dev` in your file path so it can automatically reference the libraries required to integrate
into XTMF and automatically store the compiled libraries into the module's folder.

If you do not have a development build of XTMF1 you can clone the repository from
`https://github.com/TravelModellingGroup/XTMF`.

## Building

To compile the library run the following command:

> dotnet build -c Release

This will compile the corresponding libraries into the `XTMF-Dev/Modules` directory.
