namespace SimplyMeetWasm.Models;

public record DecryptedMessageModel(Int32 Id = default, ProfileCompactModel Sender = default, MessageServerDataModel ServerData = default, MessageClientDataModel ClientData = default);
public record MessageBlockModel(List<DecryptedMessageModel> MessageList = default, Boolean IsNewDate = default);