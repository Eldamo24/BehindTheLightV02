using UnityEngine;

public interface ICocentricRing
{
    float CorrectAngle { get; }
    float CurrentAngleY { get; }
    bool IsCorrect { get; }
    void PaintSolved(Color solvedColor);
}
