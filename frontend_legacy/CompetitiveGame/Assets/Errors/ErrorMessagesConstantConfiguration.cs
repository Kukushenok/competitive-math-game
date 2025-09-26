using UnityEngine;
[CreateAssetMenu(menuName = "Game/Error messages config")]
public class ErrorMessagesConstantConfiguration : ScriptableObject, IErrorMessagesConfiguration
{
    [field:SerializeField] public string ServerErrorMessage { get; private set; }

    [field: SerializeField] public string NotPermittedMessage { get; private set; }

    [field: SerializeField] public string NotAuthorizedMessage { get; private set; }

    [field: SerializeField] public string BadRequestMessage { get; private set; }
}
