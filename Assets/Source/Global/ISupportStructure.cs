using System;

public interface ISupportStructure
{
    public event Action Waked;

    public void StartWaking();
}