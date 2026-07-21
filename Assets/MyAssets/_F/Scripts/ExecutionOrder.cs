// 実行順序の制御用定数
// DefaultExecutionOrderを各クラスに付ける
public static class ExecutionOrder
{
    public const int GameManager = -990;
    public const int UIManager = -980;
    public const int PlayerInputReader = -970;
}