{
    description = "C# dev shell";

    inputs = {
        nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-25.11";
    };

    outputs = { self, nixpkgs }: 
    let
        system = "x86_64-linux";
        pkgs = import nixpkgs {inherit system;};
    in
    {

        devShell.${system} = pkgs.mkShell {
            name = "CSharp";
            packages = with pkgs; [
                roslyn-ls
                dotnet-sdk_10
                godot-mono
            ];
        };

    };
}
