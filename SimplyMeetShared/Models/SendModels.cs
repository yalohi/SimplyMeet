namespace SimplyMeetShared.SendModels;

public interface ISendModel {};

// abstract
public abstract record SendModelBase() : ISendModel;

// models
public record ChatHistorySendModel(IEnumerable<MessageModel> Messages = default, Int32 MatchId = default, Int32 RemainingMessageCount = default, Boolean Forward = default) : SendModelBase;
