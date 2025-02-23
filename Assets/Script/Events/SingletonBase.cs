public class SingletonBase<T> where T : new()
{
    private static T instance;
    // ���̰߳�ȫ����
    private static readonly object locker = new object();
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //lockд��һ��if������Ϊֻ�и����ʵ����û����ʱ������Ҫ�������������Խ�ʡ����
                lock (locker)
                {
                    if (instance == null)
                        instance = new T();
                }
            }
            return instance;
        }
    }
}

