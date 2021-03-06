namespace NeoServer.Game.Contracts.Creatures
{
    public interface IOutfit
    {
        ushort LookType { get; set; }
        ushort Id { get; set; }
        byte Head { get; set; }
        byte Body { get; set; }
        byte Legs { get; set; }
        byte Feet { get; set; }
        byte Addon { get; set; }
    }
}