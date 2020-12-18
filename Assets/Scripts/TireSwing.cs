using UnityEngine;
using DG.Tweening;

public sealed class TireSwing : MonoBehaviour
{
    private Tween _tween;

    private void OnEnable()
    {
        AnimateBack();
    }

    private void AnimateBack()
    {
        this.transform.DOKill();
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, -30.0f);

        Vector3 rotation = this.transform.localEulerAngles;
        rotation.z = 30.0f;

        _tween = this.transform.DOLocalRotate(rotation, 1.75f, RotateMode.Fast).SetEase(Ease.InOutQuad).OnComplete(AnimateForth);
    }

    private void AnimateForth()
    {
        this.transform.DOKill();
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, 30.0f);

        Vector3 rotation = this.transform.localEulerAngles;
        rotation.z = -30.0f;

        _tween = this.transform.DOLocalRotate(rotation, 1.75f, RotateMode.Fast).SetEase(Ease.InOutQuad).OnComplete(AnimateBack);
    }

}
