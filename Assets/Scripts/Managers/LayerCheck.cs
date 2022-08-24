using UnityEngine;

public static class LayerCheck
{
    public static bool IsLayerWorld(int layer)
    {
        return layer == LayerMask.NameToLayer("World");
    }

    public static bool IsLayerGumming(int layer)
    {
        return layer == LayerMask.NameToLayer("Gumming");
    }

    public static bool IsLayerUI(int layer)
    {
        return layer == LayerMask.NameToLayer("UI");
    }

    public static bool IsLayer(int layerChecked, string layerRef)
    {
        return layerChecked == LayerMask.NameToLayer(layerRef);
    }
}
