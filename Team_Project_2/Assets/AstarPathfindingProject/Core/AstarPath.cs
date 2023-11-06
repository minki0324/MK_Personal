using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

#if NETFX_CORE
using Thread = Pathfinding.WindowsStore.Thread;
#else
using Thread = System.Threading.Thread;
#endif

[ExecuteInEditMode]
[AddComponentMenu("Pathfinding/Pathfinder")]
/// <summary>
/// A* ��� ã�� �ý����� �ٽ� ������Ʈ�Դϴ�.
/// �� Ŭ������ ��� ��� ã�� �ý����� ó���ϰ� ��� ��θ� ����ϸ� ������ �����մϴ�.
/// �� Ŭ������ �̱��� Ŭ������, ��鿡�� �ִ� �ϳ��� Ȱ�� �ν��Ͻ��� �־�� �մϴ�.
/// �Ϲ������� ���� ����ϱ�� ��ư� �ַ� <see cref="Pathfinding.Seeker"/> Ŭ������ ���� ��� ã�� �ý����� ����մϴ�.
///
/// \nosubgrouping
/// \ingroup relevant
/// </summary>
[HelpURL("http://arongranberg.com/astar/docs/class_astar_path.php")]
public class AstarPath : VersionedMonoBehaviour {
	/// <summary>A* %Pathfinding Project�� ���� ��ȣ�Դϴ�.</summary>
	public static readonly System.Version Version = new System.Version(4, 2, 17);

	/// <summary>��Ű�� �ٿ�ε� ��ġ�� ���� �����Դϴ�.</summary>
	public enum AstarDistribution { WebsiteDownload, AssetStore, PackageManager };

	/// <summary>����ڸ� �ùٸ� ������Ʈ �ٿ�ε� ��ġ�� �ȳ��ϱ� ���� �����Ϳ��� ���˴ϴ�.</summary>
	public static readonly AstarDistribution Distribution = AstarDistribution.WebsiteDownload;

	/// <summary>
	/// �� �������� A* %Pathfinding Project �귣ġ�Դϴ�.
	/// ������Ʈ�� Ȯ���Ͽ� ���� ������ ����ڰ� ���� ������Ʈ �˸��� ���� �� �ֵ��� ���˴ϴ�.
	/// </summary>
	public static readonly string Branch = "master";

	/// <summary>��� �׷��� �����͸� �����մϴ�.</summary>
	[UnityEngine.Serialization.FormerlySerializedAs("astarData")]
	public AstarData data;

	/// <summary>
	/// ��鿡�� Ȱ�� AstarPath ��ü�� ��ȯ�մϴ�.
	/// ����: AstarPath ��ü�� �ʱ�ȭ�Ǿ�߸� �� ���� �����˴ϴ� (�̴� Awake���� �߻��մϴ�).
	/// </summary>
#if UNITY_4_6 || UNITY_4_3
	public static new AstarPath active;
#else
	public static AstarPath active;
#endif

	/// <summary>Pathfinding.AstarData.graphs�� ���� ���� ����Դϴ�.</summary>
	public NavGraph[] graphs {
		get {
			if (data == null)
				data = new AstarData();
			return data.graphs;
		}
	}

	#region InspectorDebug
	/// <summary>
	/// @name Inspector - �����
	/// @{
	/// </summary>

	/// <summary>
	/// �ð������� �׷����� �� �信 ǥ���մϴ� (������ ����).
	/// </summary>
	public bool showNavGraphs = true;

	/// <summary>
	/// ��å�� �� ���� ��带 ǥ��/����ϴ�.
	///
	/// ����: �����Ϳ����� ���õ˴ϴ�.
	///
	/// ����: <see cref="unwalkableNodeDebugSize"/>
	/// </summary>
	public bool showUnwalkableNodes = true;

	/// <summary>
	/// �� �信�� ��带 �׸� �� ����� ����Դϴ�.
	///
	/// ����: �����Ϳ����� ���õ˴ϴ�.
	///
	/// ����: Pathfinding.GraphDebugMode
	/// </summary>
	public GraphDebugMode debugMode;

	/// <summary>
	/// �Ϻ� <see cref="debugMode"/> ��忡 ����� ���� ���Դϴ�.
	/// ���� ���, <see cref="debugMode"/>�� G�� ������ ���, �� ���� ��尡 ������ �������� �� ���� �����մϴ�.
	///
	/// ����: �����Ϳ����� ���õ˴ϴ�.
	///
	/// ����: <see cref="debugRoof"/>
	/// ����: <see cref="debugMode"/>
	/// </summary>
	public float debugFloor = 0;

	/// <summary>
	/// �Ϻ� <see cref="debugMode"/> ��忡 ����� ���� ���Դϴ�.
	/// ���� ���, <see cref="debugMode"/>�� G�� ������ ���, �� ���� ��尡 ������ ����� �� ���� �����մϴ�.
	///
	/// �г�Ƽ ����� ����� ���, �г�Ƽ�� <see cref="debugFloor"/>���� ���� �� ���� ������� ������ �����ǰ�,
	/// �г�Ƽ�� �� ������ ũ�ų� ���� ���� ���������� �����Ǹ� �������� ��� ������ ������ �����˴ϴ�.
	///
	/// ����: �����Ϳ����� ���õ˴ϴ�.
	///
	/// ����: <see cref="debugFloor"/>
	/// ����: <see cref="debugMode"/>
	/// </summary>
	public float debugRoof = 20000;

	/// <summary>
	/// �����Ǹ� <see cref="debugFloor"/> �� <see cref="debugRoof"/> ���� �ڵ����� �ٽ� ������ �ʽ��ϴ�.
	///
	/// ����: �����⿡���� ���õ˴ϴ�.
	/// </summary>
	public bool manualDebugFloorRoof = false;


	/// <summary>
	/// ���� Ȱ��ȭ�Ǹ�, ���� '�θ�'�� ���� ���� �׸��ϴ�.
	/// �̰��� ���� �ֱ� ��θ� ������ϱ� ���� ��带 ����Ͽ� �˻� Ʈ���� �����ݴϴ�.
	///
	/// ����: �����Ϳ����� ���õ˴ϴ�.
	///
	/// TODO: showOnlyLastPath �÷��׸� �߰��Ͽ� ��� ��带 �׸��� �� ��� ���� �ֱ� ��ο��� �湮�� ��常 �׸��� ���θ� �����մϴ�.
	/// </summary>
	public bool showSearchTree = false;

	/// <summary>
	/// ��å�� �� ���� ��� ��� ������ ť���� ũ���Դϴ�.
	///
	/// ����: �����Ϳ����� ���õ˴ϴ�. �׸��� �׷������� ������� �ʽ��ϴ�.
	/// ����: <see cref="showUnwalkableNodes"/>
	/// </summary>
	public float unwalkableNodeDebugSize = 0.3F;

	/// <summary>
	/// ����� �޽����� ���Դϴ�.
	/// ������ �����Ϸ��� (�ణ) �� ������ϰų� �ܼ� ������ �����Ϸ��� �� ������մϴ�.
	/// ���� ������ ��ų� ��� ã�� ��ũ��Ʈ�� �����ϴ� �۾��� ���� �� ���� ������ ���� ��� (���ſ�) �� ���� ������� ����մϴ�.
	/// InGame �ɼ��� �ֽ� ��� �α׸� ���� �� GUI�� ����Ͽ� ǥ���մϴ�.
	///
	/// [�¶��� �������� �̹����� ������ ���⸦ Ŭ���ϼ���]
	/// </summary>
	public PathLog logPathResults = PathLog.Normal;

	/// <summary>@}</summary>
	#endregion

	#region InspectorSettings
	/// <summary>
	/// @name Inspector - ����
	/// @{
	/// </summary>

	/// <summary>
	/// ��带 �˻��ϴ� �ִ� �Ÿ��Դϴ�.
	/// ������ ���� ����� ��带 �˻��� �� �̴� (���� ������) ���Ǵ� �ִ� �Ÿ��Դϴ�.
	///
	/// �̰��� ������ �� ���� �������� ��θ� ��û�ϴ� ��� �ش� ������ ������ �� �ִ� ���� ����� ��带 �˻��ؾ� �ϴ� ��� ���õ˴ϴ�.
	/// �� �Ÿ� ������ ��带 ã�� �� ������ ��� ��û�� �����մϴ�.
	///
	/// ����: <see cref="Pathfinding.NNConstraint.constrainDistance"/>
	/// </summary>
	public float maxNearestNodeDistance = 100;

	/// <summary>
	/// �ִ� ���� ��� �Ÿ��� �����Դϴ�.
	/// ����: <see cref="maxNearestNodeDistance"/>
	/// </summary>
	public float maxNearestNodeDistanceSqr {
		get { return maxNearestNodeDistance*maxNearestNodeDistance; }
	}

	/// <summary>
	/// ������ �� ��� �׷����� ��ĵ�ؾ� �ϴ��� �����Դϴ�.
	/// �̰��� ĳ�ÿ��� �ε��ϴ� ���� �����ϰ� ��� �׷����� ��ĵ�ؾ� �ϴ��� ���θ� �����մϴ�.
	/// �� ���� ��Ȱ��ȭ�ϸ� <see cref="Scan"/>�� ���� ȣ���ϰų� ���Ͽ��� ����� �׷����� �ε��ؾ� �մϴ�.
	///
	/// ����: <see cref="Scan"/>
	/// ����: <see cref="ScanAsync"/>
	/// </summary>
	public bool scanOnStartup = true;

	/// <summary>
	/// ��� �׷����� ���� ������ GetNearest �˻��� �����ؾ� �ϴ��� �����Դϴ�.
	/// �Ϲ������� �߰� �˻��� ù ��° ���� �˻����� ���� ����� ��带 ã�� �׷��������� ����˴ϴ�.
	/// �� ������ �Ѹ� �߰� �˻��� ��� �׷������� ����˴ϴ�.
	///
	/// ���� ��Ȱ��ȭ�� �� �� �������� ǰ���� �� ���� �˻��� �����մϴ�.
	/// 1�� �Ǵ� 2�� �̻��� �ھ ���� ��ǻ�Ϳ��� �� ���� ����ǲ�� ������ �ʴ� ��, ��κ��� ��� ��Ȱ��ȭ�ϴ� ���� �����ϴ�.
	/// ���� �ھ ���� ��ǻ�Ϳ����� �� ���� ����ǲ�� ���� �� ������ ��ΰ� ������ ������ ���� ���� ������ �ʽ��ϴ�.
	///
	/// ����: �׸��� �׷����� ����� ��� �� ������ ũ�� �߿����� �ʽ��ϴ�. �׷����� ���� ���� �˻� ��常 ����մϴ�.
	/// </summary>
	public bool fullGetNearestSearch = false;

	/// <summary>
	/// �׷����� �켱 ������ ���� �����մϴ�.
	/// �׷����� �ν����Ϳ����� ������ �������� �켱 ������ �����˴ϴ�.
	/// <see cref="prioritizeGraphsLimit"/>���� ����� ��带 ���� �׷��� �� ù ��° �׷����� ��� �׷����� �˻��ϴ� ��� ���õ˴ϴ�.
	/// </summary>
	public bool prioritizeGraphs = false;

	/// <summary>
	/// <see cref="prioritizeGraphs"/>�� ���� �Ÿ� �����Դϴ�.
	/// ����: <see cref="prioritizeGraphs"/>
	/// </summary>
	public float prioritizeGraphsLimit = 1F;

	/// <summary>
	/// �� AstarPath ��ü�� ���� ������ ���� �����Դϴ�.
	/// ���� �������� ���� ��� �� �信�� ����� �����̾�� �ϴ��� ���� ���� ���Ե˴ϴ�.
	/// </summary>
	public AstarColor colorSettings;

	/// <summary>
	/// ����� �±� �̸��Դϴ�.
	/// ����: AstarPath.FindTagNames
	/// ����: AstarPath.GetTagNames
	/// </summary>
	[SerializeField]
	protected string[] tagNames = null;

	/// <summary>
	/// �޸���ƽ(Heuristic)���� ����� �Ÿ� �Լ��Դϴ�.
	/// �޸���ƽ�� ��忡�� ��ǥ ���������� ���� ����� ��Ÿ���ϴ�.
	/// �ٸ� �޸���ƽ�� ������ ������ �ٸ� ��� �߿��� � ��θ� ���������� ������ ��Ĩ�ϴ�.
	/// �پ��� �޸���ƽ�� ���� �ڼ��� ���� �� ������ <see cref="Pathfinding.Heuristic"/>�� �����ϼ���.
	/// </summary>
	public Heuristic heuristic = Heuristic.Euclidean;

	/// <summary>
	/// �޸���ƽ�� �������Դϴ�.
	/// 1���� ���� ���� �н����δ��� �� ���� ��带 �˻��ϰ� �ϹǷ� �������ϴ�.
	/// 0�� ����ϸ� �н����ε� �˰����� Dijkstra �˰������� ���ҵ˴ϴ�. �̰��� <see cref="heuristic"/>�� None���� ������ �Ͱ� �����մϴ�.
	/// 1���� ū ���� ����ϸ� �н����ε��� (�Ϲ�������) �� ���������� ��ΰ� ����(��, ������ �ִ� ���)�� �ƴ� �� �ֽ��ϴ�.
	///
	/// ���� �� ���� 1�� �δ� ���� �����ϴ�.
	///
	/// ����: https://en.wikipedia.org/wiki/Admissible_heuristic
	/// </summary>
	public float heuristicScale = 1F;

	/// <summary>
	/// ����� �н����ε� ������ ���Դϴ�.
	/// ��Ƽ�������� �н����ε��� �ٸ� ������� �̵��Ͽ� 2�� �̻��� �ھ� ��ǻ�Ϳ��� ���ɿ� ���� ������ ���� �ʰ� �����ӷ��� ������ �� �ְ� �մϴ�.
	/// - None�� �н����ε��� Unity �����忡�� �ڷ�ƾ���� �������� �ǹ��մϴ�.
	/// - Automatic�� ������ ���� ��ǻ���� �ھ� ���� �޸𸮿� �°� �����Ϸ��� �õ��մϴ�.
	///   512MB �̸��� �޸� �Ǵ� ���� �ھ� ��ǻ���� ��� ��Ƽ�������� ������� �ʵ��� �ǵ����ϴ�.
	///
	/// ������ ��� "Auto" ���� �� �ϳ��� ����ϴ� ���� �����ϴ�.
	/// ������ ��ǻ�Ͱ� �����ϰ� 8���� �ھ �ִ� ��쿡��
	/// �ٸ� ��ǻ�ʹ� ���� �ھ� �Ǵ� ��� �ھ��̹Ƿ� 1 �Ǵ� 3���� ������ �̻� ������� �ʽ��ϴ� (�Ϲ������� Unity �����忡 �ϳ��� ���� �η��� ��).
	/// ������ ���� ��ǻ���� �ھ� ������ ���� ����ϸ� �ַ� �޸𸮸� �����ϰ� �Ǿ� �� ������ ������� �ʽ��ϴ�.
	/// �߰� �޸� ��뷮�� ������ �� ���� ������ ���� �ʽ��ϴ�. �� ������� ��� �׷����� ��� ��忡 ���� ���� ���� �����͸� �����ؾ� �մϴ�.
	/// ��ü �׷��� �����Ͱ� �ƴ����� ��� ���� ����մϴ�.
	/// �ڵ� ������ ���� ���� ��⸦ �����ϰ� �޸𸮸� �������� �ʵ��� ������ ���� �����մϴ�.
	///
	/// ���ܴ� �� ���� �ϳ� �Ǵ� �� ���� ĳ���͸� Ȱ��ȭ�ϴ� ����Դϴ�. �׷� ���� �Ϲ������� �����尡 �ϳ��� �� ����ѵ�,
	/// �� ���� �����尡 �����ϴ� �߰� ó������ �ʿ��� ���ɼ��� ���� �����ϴ�. �� ���� �����尡 �����ϴ� �ֿ� ������ �ٸ� �����忡�� �ٸ� ��θ� ����ϱ� �����Դϴ�.
	/// ���� ��� ����� ������ ���� �þ�� ������ �ӵ��� ������ �ʽ��ϴ�.
	///
	/// ����: �������� ����� �����ϴ� ��� �Ǵ� ������ ���۸� ������� �ʰ� �׷��� �����͸� ���� �����ϴ� ���,
	/// ��Ƽ�������� �̻��� ������ �����ϰ� �������� ������ �н����ε��� �۵��� ���� �� �ֽ��ϴ�.
	/// �⺻ ����(�н����ε� �ھ� �������� ����)�� ���ؼ��� �����մϴ�.
	///
	/// ����: WebGL�� �����带 ���� �������� �����Ƿ� �ش� �÷��������� �����带 ������� �ʽ��ϴ�.
	///
	/// ����: CalculateThreadCount
	/// </summary>
	public ThreadCount threadCount = ThreadCount.One;

	/// <summary>
	/// �� �����ӿ��� �н����ε��� �ҿ�� �� �ִ� �ִ� �ð�(�и���)�Դϴ�.
	/// �ּ� 500���� ��尡 �� �����ӿ��� �˻��˴ϴ� (���� �˻��� ��尡 �׸�ŭ �ִٸ�).
	/// ��Ƽ�������� ����ϴ� ��� �� ���� ���õ˴ϴ�.
	/// </summary>
	public float maxFrameTime = 1F;

	/// <summary>
	/// ������ ����Ű�� ���� �׷��� ������Ʈ�� �����ϰ� ��ġ�մϴ�.
	/// �Ѹ� �׷��� ������Ʈ�� ��ġ�Ǿ� �� ���� ����˴ϴ� (�׷��� ������Ʈ ������ <see cref="graphUpdateBatchingInterval)"/>�� ������).
	///
	/// �н����ε� �����带 ���� ������ �ʿ䰡 �����Ƿ� �н����ε� ó������ �������� ������ ��ĥ �� ������,
	/// �׷��� ������Ʈ�� ������带 ���� �� �ֽ��ϴ�.
	/// ��� �׷��� ������Ʈ�� ���������, ��� �Բ� ��ġ�Ǿ� �� ���� ����˴ϴ�.
	///
	/// �׷��� �ּ����� ��� �ð��� ���ϴ� ��� ������� ������.
	///
	/// �̰��� <see cref="UpdateGraphs"/> �޼��带 ����Ͽ� ��û�� �׷��� ������Ʈ���� ����˴ϴ�. <see cref="RegisterSafeUpdate"/> �Ǵ� <see cref="AddWorkItem"/>�� ����Ͽ� ��û�� �׷��� ������Ʈ���� ������� �ʽ��ϴ�.
	///
	/// �������� �׷��� ������Ʈ�� ��� �����Ϸ��� <see cref="FlushGraphUpdates"/>�� ȣ���� �� �ֽ��ϴ�.
	///
	/// ����: �׷��� ������Ʈ (�¶��� �������� �۵� ��ũ ����)
	/// </summary>
	public bool batchGraphUpdates = false;

	/// <summary>
	/// �� �׷��� ������Ʈ ��ġ ���� �ּ� �� ���� �ð��Դϴ�.
	/// <see cref="batchGraphUpdates"/>�� true�� ������ ���, �̰��� �� �׷��� ������Ʈ ��ġ ���� �ּ� �� ���� �ð��� �����մϴ�.
	///
	/// �н����ε� �����带 ���� ������ �ʿ䰡 �����Ƿ� �н����ε� ó������ �������� ������ ��ĥ �� ������,
	/// �׷��� ������Ʈ�� ������带 ���� �� �ֽ��ϴ�.
	/// ��� �׷��� ������Ʈ�� ���������, ��� �Բ� ��ġ�Ǿ� �� ���� ����˴ϴ�.
	///
	/// �ּ����� ��� �ð��� ���ϴ� ��� ������� ������.
	///
	/// �̰��� <see cref="UpdateGraphs"/> �޼��带 ����Ͽ� ��û�� �׷��� ������Ʈ���� ����˴ϴ�. <see cref="RegisterSafeUpdate"/> �Ǵ� <see cref="AddWorkItem"/>�� ����Ͽ� ��û�� �׷��� ������Ʈ���� ������� �ʽ��ϴ�.
	///
	/// ����: �׷��� ������Ʈ (�¶��� �������� �۵� ��ũ ����)
	/// </summary>
	public float graphUpdateBatchingInterval = 0.2F;

	/// <summary>
	/// �׷��� ������Ʈ ��ġ�� �����մϴ�.
	/// Deprecated: �� �ʵ�� 'batchGraphUpdates'�� �̸��� ����Ǿ����ϴ�.
	/// </summary>
	[System.Obsolete("This field has been renamed to 'batchGraphUpdates'")]
	public bool limitGraphUpdates { get { return batchGraphUpdates; } set { batchGraphUpdates = value; } }

	/// <summary>
	/// �׷��� ������Ʈ �ִ� �󵵿� ���� �����Դϴ�.
	/// Deprecated: �� �ʵ�� 'graphUpdateBatchingInterval'�� �̸��� ����Ǿ����ϴ�.
	/// </summary>
	[System.Obsolete("This field has been renamed to 'graphUpdateBatchingInterval'")]
	public float maxGraphUpdateFreq { get { return graphUpdateBatchingInterval; } set { graphUpdateBatchingInterval = value; } }

	/// <summary>@}</summary>
	#endregion

	#region DebugVariables
	/// <summary>
	/// @name ����� ���
	/// @{
	/// </summary>

#if ProfileAstar
/// <summary>
/// ���ø����̼� ���ۺ��� ����� ��� ���Դϴ�.\n
/// ����� ����
/// </summary>
	public static int PathsCompleted = 0;

	public static System.Int64 TotalSearchedNodes = 0;
	public static System.Int64 TotalSearchTime = 0;
#endif

	/// <summary>
	/// ���������� Scan() ȣ���� �Ϸ�� �� �ɸ� �ð��Դϴ�.
	/// �׷����� �ʹ� ���� �ڵ����� �ٽ� ��ĵ�ϴ� ���� �����ϴ� �� ���˴ϴ� (������ ����)
	/// </summary>
	public float lastScanTime { get; private set; }

	/// <summary>
	/// ������ ������ϱ� ���� ���Ǵ� ����Դϴ�.
	/// �� ��� �ڵ鷯�� ������ ��θ� ����ϴ� �� ���˴ϴ�.
	/// �����Ϳ��� gizmo�� ����Ͽ� ����� ������ �׸� �� ���˴ϴ�.
	/// </summary>
	[System.NonSerialized]
	public PathHandler debugPathData;

	/// <summary>gizmo�� ����Ͽ� ������� ��� ID�Դϴ�</summary>
	[System.NonSerialized]
	public ushort debugPathID;

	/// <summary>
	/// ������ �Ϸ�� ��ο��� ����� ���ڿ��Դϴ�.
	/// <see cref="logPathResults"/> == PathLog.InGame�� ��� ������Ʈ�˴ϴ�.
	/// </summary>
	string inGameDebugPath;

	/* @} */
	#endregion

	#region StatusVariables

	/// <summary>
	/// <see cref="isScanning"/>�� ���� ��� �ʵ��Դϴ�.
	/// System.NonSerialized�� ǥ���� �� ���� ������ �ڵ� �Ӽ��� ����� �� �����ϴ�.
	/// </summary>
	[System.NonSerialized]
	bool isScanningBacking;

	/// <summary>
	/// �׷����� ��ĵ ���� �� �����˴ϴ�.
	/// FloodFill�� �Ϸ�� ������ true�� �˴ϴ�.
	///
	/// ����: �׷��� ������Ʈ�� ȥ������ ���ʽÿ�.
	///
	/// OnPostScan���� ȣ��Ǵ� Graph Update Object�� �� �� �����ϱ� ���� ���˴ϴ�.
	///
	/// ����: IsAnyGraphUpdateQueued
	/// ����: IsAnyGraphUpdateInProgress
	/// </summary>
	public bool isScanning { get { return isScanningBacking; } private set { isScanningBacking = value; } }

	/// <summary>
	/// ���� �н����δ� ���Դϴ�.
	/// �� ���� ��θ� ����� �� �ִ� ���� ���μ��� ���� ��ȯ�մϴ�.
	/// ��Ƽ�������� ����ϴ� ��� �̴� ������ ���� ���̸�, ��Ƽ�������� ������� �ʴ� ��� �׻� 1�Դϴ� (�ڷ�ƾ�� ����ϱ� ����).
	/// ����: IsUsingMultithreading
	/// </summary>
	public int NumParallelThreads {
		get {
			return pathProcessor.NumThreads;
		}
	}

	/// <summary>
	/// ��Ƽ�������� ����ϴ��� ���θ� ��ȯ�մϴ�.
	/// \exception System.Exception ��Ƽ�������� ����ϴ��� ���θ� ������ �� ���� �� throw�˴ϴ�.
	/// �̴� ��� ã�Ⱑ �ùٸ��� �������� �ʾ��� �� �߻����� �ʾƾ� �մϴ�.
	/// ����: �̴� ���� �����尡 ���� ������ ���ο� ���� ������ ����մϴ�. A* ��ü�� ���������� ������ ������� �ʽ��ϴ�.
	/// </summary>
	public bool IsUsingMultithreading {
		get {
			return pathProcessor.IsUsingMultithreading;
		}
	}

	/// <summary>
	/// ��� ���� �׷��� ������Ʈ�� �ִ��� ���θ� ��ȯ�մϴ�.
	/// <see cref="IsAnyGraphUpdateQueued"/>�� ��� ����ϼ���.
	/// </summary>
	[System.Obsolete("Fixed grammar, use IsAnyGraphUpdateQueued instead")]
	public bool IsAnyGraphUpdatesQueued { get { return IsAnyGraphUpdateQueued; } }

	/// <summary>
	/// ��� ���� �׷��� ������Ʈ�� �ִ��� ���θ� ��ȯ�մϴ�.
	/// ����: ������Ʈ�� ���� ���� ���� false�Դϴ�.
	/// ����: �̰��� �׷��� ������Ʈ���� �ش�˴ϴ�. <see cref="RegisterSafeUpdate"/> �Ǵ� <see cref="AddWorkItem"/>�� �߰��� �ٸ� ������ �۾� �׸��� �������� �ʽ��ϴ�.
	/// </summary>
	public bool IsAnyGraphUpdateQueued { get { return graphUpdates.IsAnyGraphUpdateQueued; } }

	/// <summary>
	/// ���� �׷��� ������Ʈ�� ���� ������ ���θ� ��ȯ�մϴ�.
	/// ����: �̰��� �׷��� ������Ʈ���� �ش�˴ϴ�. <see cref="RegisterSafeUpdate"/> �Ǵ� <see cref="AddWorkItem"/>�� �߰��� �ٸ� ������ �۾� �׸��� �������� �ʽ��ϴ�.
	///
	/// ����: IsAnyWorkItemInProgress
	/// </summary>
	public bool IsAnyGraphUpdateInProgress { get { return graphUpdates.IsAnyGraphUpdateInProgress; } }

	/// <summary>
	/// ���� �۾� �׸��� ���� ������ ���θ� ��ȯ�մϴ�.
	/// ����: �̰��� ��κ��� �׷��� ������Ʈ ������ �����մϴ�.
	/// �Ϲ� �׷��� ������Ʈ, �׺�޽� �ڸ��� �� <see cref="RegisterSafeUpdate"/> �Ǵ� <see cref="AddWorkItem"/>�� �߰��� ��� �۾� �׸�� �����ϴ�.
	/// </summary>
	public bool IsAnyWorkItemInProgress { get { return workItems.workItemsInProgress; } }

	/// <summary>
	/// ���� �� �ڵ尡 �۾� �׸� ������ ����ǰ� �ִ��� ���θ� ��ȯ�մϴ�.
	/// ����: �̰��� ��κ��� �׷��� ������Ʈ ������ �����մϴ�.
	/// �Ϲ� �׷��� ������Ʈ, �׺�޽� �ڸ��� �� <see cref="RegisterSafeUpdate"/> �Ǵ� <see cref="AddWorkItem"/>�� �߰��� ��� �۾� �׸�� �����ϴ�.
	///
	/// <see cref="IsAnyWorkItemInProgress"/>�� �޸� �� ���� �۾� �׸� �ڵ尡 ���� ���� ���� true�̸�, ���� �����ӿ� ���� �۾� �׸��� ������Ʈ�ϴ� ���� true�� �ƴմϴ�.
	/// </summary>
	internal bool IsInsideWorkItem { get { return workItems.workItemsInProgressRightNow; } }

	#endregion

	#region Callbacks
	/// <summary>@name �ݹ�</summary>
	/* ��� ã�� �̺�Ʈ�� ���� �ݹ��Դϴ�.
	 * �̸� ���� ��� ã�� ���μ����� ���� �� �ֽ��ϴ�.
	 * �ݹ��� ������ ���� ����� �� �ֽ��ϴ�:
	 * \snippet MiscSnippets.cs AstarPath.Callbacks
	 */
	/// <summary>@{</summary>

	/// <summary>
	/// Awake ���Ŀ� ��� �۾��� �����ϱ� ���� ȣ��˴ϴ�.
	/// �� Awake ȣ���� ���ۿ��� ȣ��Ǹ� <see cref="active"/>�� ������ ���������� �̰� �ܿ��� �ƹ��͵� ������� �ʾҽ��ϴ�.
	/// ��Ÿ�� �߿� ������ AstarPath ���� ��ҿ� ���� �⺻ ������ �����Ϸ����� Awake������ ������ ���ִ� �� ���� ������ ����Ͻʽÿ�
	/// (��Ƽ������ ���� ���� ��)
	/// <code>
	/// // ���� �� �� AstarPath ��ü�� ����� �⺻ ������ �����մϴ�
	/// public void Start () {
	///     AstarPath.OnAwakeSettings += ApplySettings;
	///     AstarPath astar = gameObject.AddComponent<AstarPath>();
	/// }
	///
	/// public void ApplySettings () {
	///     // �븮�ڿ��� ��� ����
	///     AstarPath.OnAwakeSettings -= ApplySettings;
	///     // ���� ��� Awake ȣ�� ���Ŀ� threadCount�� ������ �� �����Ƿ� ���⿡���� ������ �� �ֽ��ϴ�.
	///     AstarPath.active.threadCount = ThreadCount.One;
	/// }
	/// </code>
	/// </summary>
	public static System.Action OnAwakeSettings;

	/// <summary>�׷����� ��ĵ�Ǳ� ���� �� �׷����� ���� ȣ��˴ϴ�.</summary>
	public static OnGraphDelegate OnGraphPreScan;

	/// <summary>�׷����� ��ĵ�� �Ŀ� �� �׷����� ���� ȣ��˴ϴ�. �ٸ� �׷����� ���� ��ĵ���� �ʾ��� �� �ֽ��ϴ�.</summary>
	public static OnGraphDelegate OnGraphPostScan;

	/// <summary>�˻��ϱ� ���� �� ��ο� ���� ȣ��˴ϴ�. ��Ƽ�������� ����� �� �����Ͻʽÿ�. �̰��� �ٸ� �����忡�� ȣ��˴ϴ�.</summary>
	public static OnPathDelegate OnPathPreSearch;

	/// <summary>�˻� �� �� ��ο� ���� ȣ��˴ϴ�. ��Ƽ�������� ����� �� �����Ͻʽÿ�. �̰��� �ٸ� �����忡�� ȣ��˴ϴ�.</summary>
	public static OnPathDelegate OnPathPostSearch;

	/// <summary>��ĵ ���� ���� ȣ��˴ϴ�.</summary>
	public static OnScanDelegate OnPreScan;

	/// <summary>��ĵ �Ŀ� ȣ��˴ϴ�. �̰��� ��ũ�� �����ϰ� �׷����� ħ�� ä��� �� ��Ÿ �� ó���� �����ϱ� ���� ȣ��˴ϴ�.</summary>
	public static OnScanDelegate OnPostScan;

	/// <summary>��ĵ�� ������ �Ϸ�Ǹ� ȣ��˴ϴ�. �̰��� Scan �Լ����� ���������� ȣ��Ǵ� ������ ȣ��˴ϴ�.</summary>
	public static OnScanDelegate OnLatePostScan;

	/// <summary>�׷����� ������Ʈ �� �� ȣ��˴ϴ�. ���� ��� �׷����� ����� ������ ��θ� �ٽ� ����ϵ��� ����մϴ�.</summary>
	public static OnScanDelegate OnGraphsUpdated;

	/// <summary>
	/// pathID�� 65536�� �Ѿ�� 0���� �缳���� �� ȣ��˴ϴ�.
	/// ����: �� �ݹ��� ȣ��� ������ �������Ƿ� �ٽ� ����Ϸ��� �ٷ� �ݹ��� ���� �Ŀ� ���� ����Ͻʽÿ�.
	/// </summary>
	public static System.Action On65KOverflow;

	/// <summary>������� ����:</summary>
	[System.ObsoleteAttribute]
	public System.Action OnGraphsWillBeUpdated;

	/// <summary>������� ����:</summary>
	[System.ObsoleteAttribute]
	public System.Action OnGraphsWillBeUpdated2;

	/* @} */
	#endregion

	#region MemoryStructures

	/// <summary>�׷��� ������Ʈ�� ó���մϴ�.</summary>
	readonly GraphUpdateProcessor graphUpdates;

	/// <summary>�� ��� ���̿� ��ΰ� �ִ����� ���� �Ϻ� ������ ����ȭ�ϱ� ���� ���� �׷����� �����մϴ�.</summary>
	internal readonly HierarchicalGraph hierarchicalGraph = new HierarchicalGraph();

	/// <summary>
	/// �׺�޽� ���� ó���մϴ�.
	/// ����: <see cref="Pathfinding.NavmeshCut"/>
	/// </summary>
	public readonly NavmeshUpdates navmeshUpdates = new NavmeshUpdates();

	/// <summary>�۾� �׸��� ó���մϴ�.</summary>
	readonly WorkItemProcessor workItems;

	/// <summary>��� ���� ��� ��θ� �����ϰ� ����մϴ�.</summary>
	PathProcessor pathProcessor;

	bool graphUpdateRoutineRunning = false;

	/// <summary>QueueGraphUpdates�� ���� �� �׷��� ������Ʈ ����� ��⿭�� �߰����� �ʵ��� �մϴ�.</summary>
	bool graphUpdatesWorkItemAdded = false;

	/// <summary>
	/// ������ �׷��� ������Ʈ�� �Ϸ�� �ð��Դϴ�.
	/// ���� �׷��� ������Ʈ�� ���� ��ġ�� �׷�ȭ�ϱ� ���� ���˴ϴ�.
	/// </summary>
	float lastGraphUpdate = -9999F;

	/// <summary>���� ��� ���� �۾� �׸��� �ִ� ��� �����մϴ�.</summary>
	PathProcessor.GraphUpdateLock workItemLock;

	/// <summary>������ ��� �Ϸ�� ��θ� ��ȯ�ϵ��� ����մϴ�.</summary>
	internal readonly PathReturnQueue pathReturnQueue;

	/// <summary>
	/// �޸���ƽ ����ȭ������ ������ �����մϴ�.
	/// ����: heuristic-opt (�۵� ��ũ�� �¶��� �������� Ȯ���Ͻʽÿ�)
	/// </summary>
	public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();

	#endregion

	/// <summary>
	/// �׷��� �˻�⸦ ǥ���ϰų� ����ϴ�.
	/// ���ο��� �����Ϳ� ���� ���˴ϴ�.
	/// </summary>
	public bool showGraphs = false;

	/// <summary>
	/// �׷��� �˻�⸦ ǥ���ϰų� ����ϴ�.
	/// ���ο��� �����Ϳ� ���� ���˴ϴ�.
	/// </summary>
	private ushort nextFreePathID = 1;

	private AstarPath () {
		pathReturnQueue = new PathReturnQueue(this);

		// pathProcessor�� null�� ���� �ʵ��� �մϴ�.
		pathProcessor = new PathProcessor(this, pathReturnQueue, 1, false);

		workItems = new WorkItemProcessor(this);
		graphUpdates = new GraphUpdateProcessor(this);

		// graphUpdates.OnGraphsUpdated�� AstarPath.OnGraphsUpdated�� �����մϴ�.
		graphUpdates.OnGraphsUpdated += () => {
			if (OnGraphsUpdated != null) {
				OnGraphsUpdated(this);
			}
		};
	}

	/// <summary>
	/// �±� �̸��� ��ȯ�մϴ�.
	/// �±� �̸� �迭�� null�̰ų� ���̰� 32�� �ƴ� ��� �� �迭�� ����� 0,1,2,3,4 ������ ä��ϴ�.
	/// </summary>
	public string[] GetTagNames () {
		if (tagNames == null || tagNames.Length != 32) {
			tagNames = new string[32];
			for (int i = 0; i < tagNames.Length; i++) {
				tagNames[i] = ""+i;
			}
			tagNames[0] = "Basic Ground";
		}
		return tagNames;
	}

	/// <summary>
	/// ���� ��� �ܺο��� AstarPath ��ü�� ���õ��� �ʾҴ��� �ʱ�ȭ�մϴ�.
	/// �̷����ϸ� <see cref="active"/> �Ӽ��� �����ǰ� ��� �׷����� ������ȭ�˴ϴ�.
	///
	/// �̰��� ���� ��忡�� �׷����� �����Ϸ��������� �׷����� ���� ������ȭ���� �ʾ��� �� �����մϴ�.
	/// ���� ��忡���� �޼���� �ƹ� �۾��� �������� �ʽ��ϴ�.
	/// </summary>
	public static void FindAstarPath () {
		if (Application.isPlaying) return;
		if (active == null) active = GameObject.FindObjectOfType<AstarPath>();
		if (active != null && (active.data.graphs == null || active.data.graphs.Length == 0)) active.data.DeserializeGraphs();
	}

	/// <summary>
	/// AstarPath ��ü�� ã�� �±� �̸��� ��ȯ�Ϸ��� �õ��մϴ�.
	/// AstarPath ��ü�� ã�� �� ������ ���̰� 1�� ���� �޽����� �����ϴ� �迭�� ��ȯ�մϴ�.
	/// </summary>
	public static string[] FindTagNames () {
		FindAstarPath();
		return active != null? active.GetTagNames () : new string[1] { "There is no AstarPath component in the scene" };
	}

	/// <summary>���� ���� ��� ID�� ��ȯ�մϴ�</summary>
	internal ushort GetNextPathID () {
		if (nextFreePathID == 0) {
			nextFreePathID++;

			if (On65KOverflow != null) {
				System.Action tmp = On65KOverflow;
				On65KOverflow = null;
				tmp();
			}
		}
		return nextFreePathID++;
	}

	void RecalculateDebugLimits () {
		debugFloor = float.PositiveInfinity;
		debugRoof = float.NegativeInfinity;

		bool ignoreSearchTree = !showSearchTree || debugPathData == null;
		for (int i = 0; i < graphs.Length; i++) {
			if (graphs[i] != null && graphs[i].drawGizmos) {
				graphs[i].GetNodes(node => {
					if (node.Walkable && (ignoreSearchTree || Pathfinding.Util.GraphGizmoHelper.InSearchTree(node, debugPathData, debugPathID))) {
						if (debugMode == GraphDebugMode.Penalty) {
							debugFloor = Mathf.Min(debugFloor, node.Penalty);
							debugRoof = Mathf.Max(debugRoof, node.Penalty);
						} else if (debugPathData != null) {
							var rnode = debugPathData.GetPathNode(node);
							switch (debugMode) {
							case GraphDebugMode.F:
								debugFloor = Mathf.Min(debugFloor, rnode.F);
								debugRoof = Mathf.Max(debugRoof, rnode.F);
								break;
							case GraphDebugMode.G:
								debugFloor = Mathf.Min(debugFloor, rnode.G);
								debugRoof = Mathf.Max(debugRoof, rnode.G);
								break;
							case GraphDebugMode.H:
								debugFloor = Mathf.Min(debugFloor, rnode.H);
								debugRoof = Mathf.Max(debugRoof, rnode.H);
								break;
							}
						}
					}
				});
			}
		}

		if (float.IsInfinity(debugFloor)) {
			debugFloor = 0;
			debugRoof = 1;
		}

		// �� ���� �������� �ʵ����Ͻʽÿ�. �̷����ϸ� ���� ������ �����մϴ�.
		if (debugRoof-debugFloor < 1) debugRoof += 1;
	}

	Pathfinding.Util.RetainedGizmos gizmos = new Pathfinding.Util.RetainedGizmos();

	/// <summary>
	/// �׷��� �����⿡�� OnDrawGizmos�� ȣ���մϴ�.
	/// </summary>
	private void OnDrawGizmos () {
		// �̱��� ������ �����ǵ��� �մϴ�.
		// Awake �޼��尡 ȣ����� ���� ��쿡�� �ش��� �� �ֽ��ϴ�.
		if (active == null) active = this;

		if (active != this || graphs == null) {
			return;
		}

		// Unity������ ���콺�� ��ü�� Ŭ���Ͽ� �� �信�� ��ü�� ������ �� �ֽ��ϴ�.
		// �׷��� �׷��� ������ �̸� �����մϴ�. ���⿡�� �޽ø� �׸� ��� ����ڴ� �ڿ� �ִ� ��ü�� ������ �� �����ϴ�.
		// (�Ƹ��� Unity�� ��κ��� ����� �׸��� ���� Graphics.DrawMeshNow�� ����ϰ� �ֱ� ������
		// AstarPath ���� ��ҿ� ����� �������� ���� ���Դϴ�). ������ �� ��ŷ�� �߻��ϴ� ���
		// Event.current.type�� 'mouseUp'�� �� ������ ����˴ϴ�. ���� OnDrawGizmos �߿���
		// ���� �̺�Ʈ�� �����Ͽ� ����� �� ��ŷ�� ��ȣ �ۿ����� �ʵ����մϴ�.
		// �̷��� ���� ������ ȭ�鿡 ������ ��ġ�� ��쿡�� �߻��ϱ� ������ �ð����� ������ �����ϴ�.
		// �׽�Ʈ ��� OnDrawGizmos �߿� �߻��� �� �ִ� �̺�Ʈ�� mouseUp �� repaint �̺�Ʈ�� �ִ� ������ ���Դϴ�.
		if (Event.current.type != EventType.Repaint) return;

		colorSettings.PushToStatic(this);

		AstarProfiler.StartProfile("OnDrawGizmos");

		if (workItems.workItemsInProgress || isScanning) {
			// �׷��� ������Ʈ ���̹Ƿ� �׷��� ������ ���� ��ȿ���� ���� �� �ֽ��ϴ�.
			// ���� ���� �����Ӱ� ������ ���� �׸��ϴ�.
			// ���� ���� ���� ���� ī�޶� �ְų� (�Ǵ� �����Ϳ��� �� ��� ���� �䰡 �ִ� ���) �츮��
			// �޽��� �� �� ����� ���� �ٸ� ī�޶� ���� ���� �޽��� �ٽ� �׸��ϴ�.
			// �̰��� ������ ����� ����ŵ�ϴ�.
			gizmos.DrawExisting();
		} else {
			if (showNavGraphs && !manualDebugFloorRoof) {
				RecalculateDebugLimits();
			}

			Profiler.BeginSample("Graph.OnDrawGizmos");
			// ��� �׷����� ��ȯ�ϰ� ����� �׸��ϴ�.
			for (int i = 0; i < graphs.Length; i++) {
				if (graphs[i] != null && graphs[i].drawGizmos)
					graphs[i].OnDrawGizmos(gizmos, showNavGraphs);
			}
			Profiler.EndSample();

			if (showNavGraphs) {
				euclideanEmbedding.OnDrawGizmos();
				if (debugMode == GraphDebugMode.HierarchicalNode) hierarchicalGraph.OnDrawGizmos(gizmos);
			}
		}

		gizmos.FinalizeDraw();

		AstarProfiler.EndProfile("OnDrawGizmos");
	}

#if !ASTAR_NO_GUI
	/// <summary>
	/// InGame ������� �׸��ϴ� (Ȱ��ȭ �� ���), 'L' Ű�� ������ fps�� ǥ�õ˴ϴ�.
	/// See: <see cref="logPathResults"/> PathLog
	/// </summary>
	private void OnGUI () {
		if (logPathResults == PathLog.InGame && inGameDebugPath != "") {
			GUI.Label(new Rect(5, 5, 400, 600), inGameDebugPath);
		}
	}
#endif

	/// <summary>
	/// ��� ����� �α׿� ����մϴ�. ��� ������ <see cref="logPathResults"/>�� ����Ͽ� ������ �� �ֽ��ϴ�.
	/// See: <see cref="logPathResults"/>
	/// See: PathLog
	/// See: Pathfinding.Path.DebugString
	/// </summary>
	private void LogPathResults (Path path) {
		if (logPathResults != PathLog.None && (path.error || logPathResults != PathLog.OnlyErrors)) {
			string debug = (path as IPathInternals).DebugString(logPathResults);

			if (logPathResults == PathLog.InGame) {
				inGameDebugPath = debug;
			} else if (path.error) {
				Debug.LogWarning(debug);
			} else {
				Debug.Log(debug);
			}
		}
	}

	/// <summary>
	/// �۾� �׸��� ����Ǿ�� �ϴ��� Ȯ���� ���� ��� ã�⸦ �����ϰ�,
	/// ���� ��û�� ��ũ��Ʈ�� ���� ��θ� ��ȯ�մϴ�.
	///
	/// See: PerformBlockingActions
	/// See: PathProcessor.TickNonMultithreaded
	/// See: PathReturnQueue.ReturnPaths
	/// </summary>
	private void Update () {
		// �� Ŭ������ [ExecuteInEditMode] �Ӽ��� ����մϴ�.
		// ���� Update�� ���� ������ �ʾƵ� ȣ��˴ϴ�.
		// �÷��� ��尡 �ƴ� ��� �ƹ� �۾��� �������� ���ʽÿ�.
		if (!Application.isPlaying) return;

		navmeshUpdates.Update();

		// ��ĳ�� ���� �ƴ� ��� �׷��� ������Ʈ�� ���� ���ŷ �۾� ����
		if (!isScanning) {
			PerformBlockingActions();
		}

		// ���� �������� ������� �ʴ� ��� ��� ���
		pathProcessor.TickNonMultithreaded();

		// ���� ��� ��ȯ
		pathReturnQueue.ReturnPaths(true);
	}

	private void PerformBlockingActions (bool force = false) {
		if (workItemLock.Held && pathProcessor.queue.AllReceiversBlocked) {
			// ���ŷ �۾��� �����ϱ� ���� ��� ��θ� ��ȯ�մϴ�.
			// �̷����ϸ� �׷����� ����Ǿ� ��ȯ�� ��ΰ� ��ȿȭ�� �� �����Ƿ�(�ּ��� ��常)
			// ��ΰ� �Ϸ�Ǿ����ϴ�.
			pathReturnQueue.ReturnPaths(false);

			Profiler.BeginSample("Work Items");
			if (workItems.ProcessWorkItems(force)) {
				// �� �ܰ迡�� �� �̻� �۾� �׸��� �����Ƿ� ��� ã�� �����带 �ٽ� �����մϴ�.
				workItemLock.Release();
			}
			Profiler.EndSample();
		}
	}

	/// <summary>
	/// ��� ã�Ⱑ �Ͻ� ������ �� ó���ǵ��� ��⿭�� �۾� �׸��� �߰��մϴ�.
	/// ���� �޼���� ������ �����մϴ�.
	/// <code>
	/// AddWorkItem(new AstarWorkItem(callback));
	/// </code>
	///
	/// See: <see cref="AddWorkItem(AstarWorkItem)"/>
	/// </summary>
	public void AddWorkItem (System.Action callback) {
		AddWorkItem(new AstarWorkItem(callback));
	}

	/// <summary>
	/// ��� ã�Ⱑ �Ͻ� ������ �� ó���ǵ��� ��⿭�� �۾� �׸��� �߰��մϴ�.
	/// ���� �޼���� ������ �����մϴ�.
	/// <code>
	/// AddWorkItem(new AstarWorkItem(callback));
	/// </code>
	///
	/// See: <see cref="AddWorkItem(AstarWorkItem)"/>
	/// </summary>
	public void AddWorkItem (System.Action<IWorkItemContext> callback) {
		AddWorkItem(new AstarWorkItem(callback));
	}

	/// <summary>
	/// ��� ã�Ⱑ �Ͻ� ������ �� ó���ǵ��� ��⿭�� �۾� �׸��� �߰��մϴ�.
	///
	/// �۾� �׸��� ��带 ������Ʈ�ϴ� ���� ������ ��쿡 ����˴ϴ�. �̴� ��� �˻� ���̿��� ���ǵ˴ϴ�.
	/// �� ���� �����带 ����ϴ� ��� �� �޼��带 ���� ȣ���ϸ� �����忡�� ���� ���ð����� ���� ��� ã�� ������ ���ϵ� �� �ֽ��ϴ�.
	/// ���⼭ ������ CPU ������ ���� ������� �ʴ´ٴ� �ǹ̰� �ƴ϶� �ʴ� ��� ���� �Ƹ��� �پ� �� ���Դϴ�
	/// (�׷��� ������ �ӵ��� �ణ ������ �� ����).
	///
	/// �� �Լ��� �ַ� Unity�� ���� �����忡���� ȣ���ؾ� �մϴ� (��, �Ϲ����� ���� �ڵ�).
	///
	/// <code>
	/// AstarPath.active.AddWorkItem(new AstarWorkItem(() => {
	///     // ���⿡�� �׷����� ������Ʈ�ص� �����մϴ�.
	///     var node = AstarPath.active.GetNearest(transform.position).node;
	///     node.Walkable = false;
	/// }));
	/// </code>
	///
	/// <code>
	/// AstarPath.active.AddWorkItem(() => {
	///     // ���⿡�� �׷����� ������Ʈ�ص� �����մϴ�.
	///     var node = AstarPath.active.GetNearest(transform.position).node;
	///     node.position = (Int3)transform.position;
	/// });
	/// </code>
	///
	/// See: <see cref="FlushWorkItems"/>
	/// </summary>
	public void AddWorkItem (AstarWorkItem item) {
		workItems.AddWorkItem(item);

		// ��� ã�⸦ �����ϰ� �۾� �׸��� ó���մϴ�.
		if (!workItemLock.Held) {
			workItemLock = PausePathfindingSoon();
		}

#if UNITY_EDITOR
		// �÷��� ���� �ƴ� ��� ��� ����
		if (!Application.isPlaying) {
			FlushWorkItems();
		}
#endif
	}

	#region GraphUpdateMethods

	/// <summary>
	/// ������ �� ���� ��� ���� �׷��� ������Ʈ�� �����մϴ�. <see cref="batchGraphUpdates"/>�� ������� ȣ��˴ϴ�.
	/// ���� �� ȣ���ص� ���� ���� �ݹ��� �������� �ʽ��ϴ�.
	/// �� �Լ��� �׷��� ������Ʈ �ð� ���Ѱ� ������� Ư�� �׷��� ������Ʈ�� ������ �� ���� �����Ϸ��� ��� �����մϴ�.
	/// ����� �� �Լ��� ������Ʈ�� �Ϸ�� ������ ���ܵ��� ������, <see cref="batchGraphUpdates"/> ���� �ð��� ��ȸ�ϱ⸸ �մϴ�.
	///
	/// ����: <see cref="FlushGraphUpdates"/>
	/// </summary>
	public void QueueGraphUpdates () {
		if (!graphUpdatesWorkItemAdded) {
			graphUpdatesWorkItemAdded = true;
			var workItem = graphUpdates.GetWorkItem();

			// �׷��� ������Ʈ �۾� �׸��� �߰��մϴ�. ���� graphUpdatesWorkItemAdded �÷��׸� false�� ������ ���� �׷��� ������Ʈ�� ó���մϴ�.
			AddWorkItem(new AstarWorkItem(() => {
				graphUpdatesWorkItemAdded = false;
				lastGraphUpdate = Time.realtimeSinceStartup;

				workItem.init();
			}, workItem.update));
		}
	}

	/// <summary>
	/// �׷��� ������Ʈ�� �������� ���� �ð� ���� ����մϴ�.
	/// batchGraphUpdates�� ������ ��� ��� ã�� �����带 ��� �����ϰ� ť�� ��� ���� ȣ���� �� ���� ����Ϸ��� �մϴ�.
	/// </summary>
	IEnumerator DelayedGraphUpdate () {
		graphUpdateRoutineRunning = true;

		yield return new WaitForSeconds(graphUpdateBatchingInterval-(Time.realtimeSinceStartup-lastGraphUpdate));
		QueueGraphUpdates();
		graphUpdateRoutineRunning = false;
	}

	/// <summary>
	/// ���� �ð� �Ŀ� bounds ���� ��� �׷����� ������Ʈ�մϴ�.
	/// �׷����� ������ �� ���� ������Ʈ�˴ϴ�.
	///
	/// ����: <see cref="FlushGraphUpdates"/>
	/// ����: batchGraphUpdates
	/// ����: graph-updates(�۵� ��ũ �¶��� �������� ����)
	/// </summary>
	public void UpdateGraphs (Bounds bounds, float delay) {
		UpdateGraphs(new GraphUpdateObject(bounds), delay);
	}

	/// <summary>
	/// delay �� �Ŀ� GraphUpdateObject�� ����Ͽ� ��� �׷����� ������Ʈ�մϴ�.
	/// �̸� ����Ͽ� ���� ��� ���� ���� ��� ��带 ���� ���ϵ��� ����ų� �� ���� �г�Ƽ�� ������ �� �ֽ��ϴ�.
	///
	/// ����: <see cref="FlushGraphUpdates"/>
	/// ����: batchGraphUpdates
	/// ����: graph-updates(�۵� ��ũ �¶��� �������� ����)
	/// </summary>
	public void UpdateGraphs (GraphUpdateObject ob, float delay) {
		StartCoroutine(UpdateGraphsInternal(ob, delay));
	}

	/// <summary>���� �ð� �Ŀ� ��� �׷����� ������Ʈ�մϴ�.</summary>
	IEnumerator UpdateGraphsInternal (GraphUpdateObject ob, float delay) {
		yield return new WaitForSeconds(delay);
		UpdateGraphs(ob);
	}

	/// <summary>
	/// bounds ���� ��� �׷����� ������Ʈ�մϴ�.
	/// �׷����� ������ �� ���� ������Ʈ�˴ϴ�.
	///
	/// �̰��� ������ �����մϴ�.
	/// <code>
	/// UpdateGraphs(new GraphUpdateObject(bounds));
	/// </code>
	///
	/// ����: <see cref="FlushGraphUpdates"/>
	/// ����: batchGraphUpdates
	/// ����: graph-updates(�۵� ��ũ �¶��� �������� ����)
	/// </summary>
	public void UpdateGraphs (Bounds bounds) {
		UpdateGraphs(new GraphUpdateObject(bounds));
	}

	/// <summary>
	/// GraphUpdateObject�� ����Ͽ� ��� �׷����� ������Ʈ�մϴ�.
	/// ���� ��� ���� ���� ��� ��带 ���� ���ϰ� ����ų� �� ���� �г�Ƽ�� �����ϴ� �� ����� �� �ֽ��ϴ�.
	/// �׷����� ������ �� ���� (batchGraphUpdates�� ����) ������Ʈ�˴ϴ�.
	///
	/// ����: <see cref="FlushGraphUpdates"/>
	/// ����: batchGraphUpdates
	/// ����: graph-updates(�۵� ��ũ �¶��� �������� ����)
	/// </summary>
	public void UpdateGraphs (GraphUpdateObject ob) {
		if (ob.internalStage != GraphUpdateObject.STAGE_CREATED) {
			throw new System.Exception("You are trying to update graphs using the same graph update object twice. Please create a new GraphUpdateObject instead.");
		}
		ob.internalStage = GraphUpdateObject.STAGE_PENDING;
		graphUpdates.AddToQueue(ob);

		// �׷��� ������Ʈ�� �����ؾ� �ϴ� ���, �׷����� ������Ʈ�ؾ� �� ������ ����ϴ� �ڷ�ƾ�� �����մϴ�.
		if (batchGraphUpdates && Time.realtimeSinceStartup-lastGraphUpdate < graphUpdateBatchingInterval) {
			if (!graphUpdateRoutineRunning) {
				StartCoroutine(DelayedGraphUpdate());
			}
		} else {
			// �׷��� ������ �׷��� ������Ʈ�� ������ �� ���� ����Ǿ�� �մϴ�.
			QueueGraphUpdates();
		}
	}

	/// <summary>
	/// �׷��� ������Ʈ�� ���� �����ӿ��� �Ϸ��ϵ��� �����մϴ�.
	/// �̷��� �ϸ� ��� ã�� �����尡 ���� ��� ���� ��θ� ����ϵ��� �����ϰ� �Ͻ� �����մϴ� (�ִ� ���) .
	/// ��� �����尡 �Ͻ� �����Ǹ� �׷��� ������Ʈ�� ����˴ϴ�.
	/// ��� ã�� ������ ���Ͻ�ų �� �ִ� (�ʴ� ���� �����尡 ���θ� ��ٸ��� ���) �� �Լ��� ���� (�ʴ� ���� ��) ����ϸ� FPS�� ������ �� �ֽ��ϴ�.
	/// �׷��� ������ �׷��� ������ �ʿ�� ���� ���Դϴ�.
	///
	/// ����: �� �Լ��� ���� <see cref="FlushWorkItems"/>�� ���������� ������ �� �ڼ��մϴ�.
	/// �� �Լ��� �׷��� ������Ʈ�� ���� �ð� ���� ������ �������մϴ�.
	/// �� ������ �׷��� ������Ʈ�� �۾� �׸��� ����Ͽ� �����Ǳ� �����Դϴ�.
	/// ���� �� �Լ��� ȣ���ϸ� �ٸ� �۾� �׸� (�ִ� ���)�� ����˴ϴ�.
	///
	/// ��� ���� �׷��� ������Ʈ�� ������ (�ٸ� �۾� �׸� ������� ����) �ƹ� �۾��� �������� �ʽ��ϴ�.
	/// </summary>
	public void FlushGraphUpdates () {
		if (IsAnyGraphUpdateQueued) {
			QueueGraphUpdates();
			FlushWorkItems();
		}
	}

	#endregion

	/// <summary>
	/// ���� �����ӿ��� ��� �۾� �׸��� �Ϸ��ϵ��� �����մϴ�.
	/// �̷��� �ϸ� ��� �۾� �׸��� ��� ����˴ϴ�.
	/// ��� ã�� �����尡 ���� ��� ���� ��θ� ����ϵ��� �����ϰ� �Ͻ� �����մϴ� (�ִ� ���).
	/// ��� �����尡 �Ͻ� �����Ǹ� �۾� �׸��� ����˴ϴ� (��: �׷��� ������Ʈ).
	///
	/// ���: �̰��� ���� (�ʴ� ���� ��) ����ϸ� ���θ� ��ٸ��� ���� ������ ������ FPS�� ������ �� �ֽ��ϴ�.
	/// �׷��� �Ƹ��� �׷��� ������ �ʿ�� ���� ���Դϴ�.
	///
	/// ����: �̰��� ���� (<see cref="FlushGraphUpdates"/>�� ���� ������) ���������� ������ �� �ڼ��մϴ�.
	///
	/// ��� ���� ������ �۾� �׸��� ������ �ƹ� �۾��� �������� �ʽ��ϴ�.
	/// </summary>
	public void FlushWorkItems () {
		if (workItems.anyQueued) {
			var graphLock = PausePathfinding();
			PerformBlockingActions(true);
			graphLock.Release();
		}
	}

	/// <summary>
	/// �۾� �׸��� ����ǵ��� Ȯ���մϴ�.
	///
	/// ����: AddWorkItem
	///
	/// Deprecated: ��� <see cref="FlushWorkItems()"/>�� ����մϴ�.
	/// </summary>
	/// <param name="unblockOnComplete">true�� ��� �۾� �׸��� ��� �Ϸ��� �Ŀ� ��� ��� ã�⸦ ������ �� �ֽ��ϴ�.</param>
	/// <param name="block">true�� ��� �Ϲ������� ���� �����ӿ� ���� �Ϸ�Ǵ� �۾� �׸����� ȣ�� �߿� �Ϸ��ϵ��� �����մϴ�.
	///              false�̸��� ȣ�� �Ŀ� ���� �۾��� ���� ���� �� �ֽ��ϴ�.</param>
	[System.Obsolete("FlushWorkItems() ��� ����ϼ���")]
	public void FlushWorkItems (bool unblockOnComplete, bool block) {
		var graphLock = PausePathfinding();

		// Run tasks
		PerformBlockingActions(block);
		graphLock.Release();
	}

	/// <summary>
	/// ����� ������ ���� ����մϴ�.
	/// count�� �ڵ��� �ƴ� ���, count�� int�� ĳ�����Ͽ� ��ȯ�մϴ�.
	/// ��ȯ ��: ����� ������ ���� �����ϴ� int�Դϴ�. 0�� ������ ������ ��� ��� ã�⿡ ��⿭�� ����ؾ� ���� �ǹ��մϴ�.
	///
	/// count�� Automatic�� ������ ���, ���� �ý����� ���μ��� ���� �޸𸮿� ����� ���� ��ȯ�մϴ�.
	/// �޸𸮰� <= 512MB �̰ų� �� �ھ <= 1�̸� 0�� ��ȯ�մϴ�. �޸𸮰� <= 1024�̸� �����带 �ִ� 2���� Ŭ�����մϴ�.
	/// �׷��� ������ �� �ھ� ���� 6���� Ŭ�����մϴ�.
	///
	/// WebGL������ �� �޼���� �׻� 0�� ��ȯ�մϴ�.
	/// </summary>
	public static int CalculateThreadCount (ThreadCount count) {
#if UNITY_WEBGL
		return 0;
#else
		if (count == ThreadCount.AutomaticLowLoad || count == ThreadCount.AutomaticHighLoad) {
			int logicalCores = Mathf.Max(1, SystemInfo.processorCount);
			int memory = SystemInfo.systemMemorySize;

			if (memory <= 0) {
				Debug.LogError("Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
				memory = 1024;
			}

			if (logicalCores <= 1) return 0;
			if (memory <= 512) return 0;

			return 1;
		} else {
			return (int)count > 0 ? 1 : 0;
		}
#endif
	}

	/// <summary>
	/// �ʿ��� ������ �����ϰ� �׷����� ��ĵ�մϴ�.
	/// Initialize�� ȣ���ϰ� ReturnPaths �ڷ�ƾ�� �����ϰ� ��� �׷����� ��ĵ�մϴ�.
	/// ���� ��Ƽ�������� ����ϴ� ��� �����带 �����մϴ�.
	/// ����: <see cref="OnAwakeSettings"/>
	/// </summary>
	protected override void Awake () {
		base.Awake();
		// �̱��� ������ �����ǵ��� �ſ� �߿��մϴ�.
		active = this;

		if (FindObjectsOfType(typeof(AstarPath)).Length > 1) {
			Debug.LogError("�� ���� �ϳ� �̻��� AstarPath ���� ��Ҹ� ���� ���� ���ƾ� �մϴ�.\n" +
		  "�̷��� �ϸ� AstarPath ���� ��Ұ� �̱��� ������ �߽����� ����Ǳ� ������ �ɰ��� ������ �߻��� �� �ֽ��ϴ�.");
		}

		// GUILayout�� ������� �ʵ��� �����Ͽ� ������ ���Դϴ�. OnGUI ȣ�⿡�� ������ �ʽ��ϴ�.
		useGUILayout = false;

		// �� Ŭ������ [ExecuteInEditMode] �Ӽ��� ����ϹǷ� Awake�� ���� ������ ���� ���� ȣ��˴ϴ�.
		// �÷��� ��尡 �ƴ� ���� �ƹ� �۾��� �������� �ʽ��ϴ�.
		if (!Application.isPlaying) return;

		if (OnAwakeSettings != null) {
			OnAwakeSettings();
		}

		// ��ĵ�ϱ� ���� ��� �׷��� �����ڰ� Ȱ��ȭ�Ǿ����� Ȯ�� (��ũ��Ʈ ���� ���� ���� ����)
		GraphModifier.FindAllModifiers();
		RelevantGraphSurface.FindAllGraphSurfaces();

		InitializePathProcessor();
		InitializeProfiler();
		ConfigureReferencesInternal();
		InitializeAstarData();

		// �۾� �׸� �÷���, �׷��� ������ �ε带���� InitializeAstarData�� �߰� �� �� ����
		FlushWorkItems();

		euclideanEmbedding.dirty = true;

		navmeshUpdates.OnEnable();

		if (scanOnStartup && (!data.cacheStartup || data.file_cachedStartup == null)) {
			Scan();
		}
	}

	/// <summary><see cref="pathProcessor"/> �ʵ带 �ʱ�ȭ�մϴ�.</summary>
	void InitializePathProcessor () {
		int numThreads = CalculateThreadCount(threadCount);

		// �÷��� ��� �̿ܿ����� ��� ���� �������̹Ƿ� �����带 ������� �ʽ��ϴ�.
		if (!Application.isPlaying) numThreads = 0;

		// �ܼ��� ������� ���� �����带 �����Ϸ��� �õ� ����
		if (numThreads > 1) {
			threadCount = ThreadCount.One;
			numThreads = 1;
		}

		int numProcessors = Mathf.Max(numThreads, 1);
		bool multithreaded = numThreads > 0;
		pathProcessor = new PathProcessor(this, pathReturnQueue, numProcessors, multithreaded);

		pathProcessor.OnPathPreSearch += path => {
			var tmp = OnPathPreSearch;
			if (tmp != null) tmp(path);
		};

		pathProcessor.OnPathPostSearch += path => {
			LogPathResults(path);
			var tmp = OnPathPostSearch;
			if (tmp != null) tmp(path);
		};

		// Sent every time the path queue is unblocked
		pathProcessor.OnQueueUnblocked += () => {
			if (euclideanEmbedding.dirty) {
				euclideanEmbedding.RecalculateCosts();
			}
		};

		if (multithreaded) {
			graphUpdates.EnableMultithreading();
		}
	}

	/// <summary>������ ���� Ȯ���� �����մϴ�.</summary>
	internal void VerifyIntegrity () {
		if (active != this)
		{
			throw new System.Exception("�̱��� ������ �������ϴ�. ���� AstarPath ��ü�� �ϳ��� �νʽÿ�.");
		}

		if (data == null)
		{
			throw new System.NullReferenceException("�����Ͱ� null�Դϴ�... A*�� ����� �������� �ʾҽ��ϱ�?");
		}

		if (data.graphs == null) {
			data.graphs = new NavGraph[0];
			data.UpdateShortcuts();
		}
	}

	/// <summary>\cond internal</summary>
	/// <summary>
	/// <see cref="active"/>�� �� ��ü�� �����ǰ� <see cref="data"/>�� null�� �ƴ��� Ȯ���ϴ� ���� �޼����Դϴ�.
	/// ���� <see cref="colorSettings"/>�� OnEnable�� ȣ���ϰ� ������.userConnections�� �ʱ�ȭ���� ���� ��� �ʱ�ȭ�մϴ�.
	///
	/// ���: �ַ� �ý��� ���ο��� ����ϵ��� �Ǿ� �ֽ��ϴ�.
	/// </summary>
	public void ConfigureReferencesInternal () {
		active = this;
		data = data ?? new AstarData();
		colorSettings = colorSettings ?? new AstarColor();
		colorSettings.PushToStatic(this);
	}
	/// <summary>\endcond</summary>

	/// <summary>AstarProfiler.InitializeFastProfile�� ȣ���մϴ�.</summary>
	void InitializeProfiler () {
		AstarProfiler.InitializeFastProfile(new string[14] {
			"Prepare",          //0
			"Initialize",       //1
			"CalculateStep",    //2
			"Trace",            //3
			"Open",             //4
			"UpdateAllG",       //5
			"Add",              //6
			"Remove",           //7
			"PreProcessing",    //8
			"Callback",         //9
			"Overhead",         //10
			"Log",              //11
			"ReturnPaths",      //12
			"PostPathCallback"  //13
		});
	}

	/// <summary>
	/// AstarData Ŭ������ �ʱ�ȭ�մϴ�.
	/// �׷��� ������ �˻��ϰ� <see cref="data"/> �� ��� �׷����� Awake�� ȣ���մϴ�.
	///
	/// ����: AstarData.FindGraphTypes
	/// </summary>
	void InitializeAstarData () {
		data.FindGraphTypes();
		data.Awake();
		data.UpdateShortcuts();
	}

	/// <summary>�޸� ������ �����ϱ� ���� �޽ø� �����մϴ�.</summary>
	void OnDisable () {
		gizmos.ClearCache();
	}

	/// <summary>
	/// ���� �� ��Ÿ �׸��� �����ϰ� �׷����� �����մϴ�.
	/// AstarPath ��ü�� �ı��� �� ��� �ݹ�� ���� ���� ������ �������ϴ�.
	/// </summary>
	void OnDestroy () {
		// �� Ŭ������ [ExecuteInEditMode] �Ӽ��� ����ϹǷ� OnDestroy�� ���� ������ ���� ���� ȣ��˴ϴ�.
		// �÷��� ��尡 �ƴ� ���� �ƹ� �۾��� �������� �ʽ��ϴ�.
		if (!Application.isPlaying) return;

		if (logPathResults == PathLog.Heavy)
			Debug.Log("+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");

		if (active != this) return;

		// ���� ��� ����� �Ϸ� �� ������ ��� ã�� ������ ����
		PausePathfinding();

		navmeshUpdates.OnDisable();

		euclideanEmbedding.dirty = false;
		FlushWorkItems();

		// �� AstarPath �ν��Ͻ��� �� �̻� ��� ȣ���� �������� �ʽ��ϴ�.
		// �̷� ���� ��� ��� ã�� �����尡 ����˴ϴ� (�ִ� ���)
		pathProcessor.queue.TerminateReceivers();

		if (logPathResults == PathLog.Heavy)
			Debug.Log("Processing Possible Work Items");

		// �׷��� ������Ʈ ������ ���� (���� ���� ���)
		graphUpdates.DisableMultithreading();

		// ��� ã�� ������ ���� �õ�
		pathProcessor.JoinThreads();

		if (logPathResults == PathLog.Heavy)
			Debug.Log("Returning Paths");


		// ��� ��� ��ȯ
		pathReturnQueue.ReturnPaths(false);

		if (logPathResults == PathLog.Heavy)
			Debug.Log("Destroying Graphs");


		// �׷��� ������ ����
		data.OnDestroy();

		if (logPathResults == PathLog.Heavy)
			Debug.Log("Cleaning up variables");

		// ���� ����, ���� ������ �����ؾ� ���� ������ �̻��� �����͸� ���� ���� ���Դϴ�.

		// ��� �ݹ� �����
		OnAwakeSettings			= null;
		OnGraphPreScan          = null;
		OnGraphPostScan         = null;
		OnPathPreSearch         = null;
		OnPathPostSearch        = null;
		OnPreScan               = null;
		OnPostScan              = null;
		OnLatePostScan          = null;
		On65KOverflow           = null;
		OnGraphsUpdated         = null;

		active = null;
	}

	#region ��ĵ �޼���

	/// <summary>
	/// ���ο� ���� ��� �ε����� ��ȯ�մϴ�.
	/// ���: �� �޼���� ���� ȣ���ؼ��� �ȵ˴ϴ�. GraphNode �����ڿ��� ���˴ϴ�.
	/// </summary>
	internal int GetNewNodeIndex () {
		return pathProcessor.GetNewNodeIndex();
	}

	/// <summary>
	/// ����� �ӽ� ��� �����͸� �ʱ�ȭ�մϴ�.
	/// ���: �� �޼���� ���� ȣ���ؼ��� �ȵ˴ϴ�. GraphNode �����ڿ��� ���˴ϴ�.
	/// </summary>
	internal void InitializeNode (GraphNode node) {
		pathProcessor.InitializeNode(node);
	}

	/// <summary>
	/// �־��� ��带 �ı��ϱ� ���� ���� �޼����Դϴ�.
	/// �� �޼���� ��尡 �׷������� �и��� �Ŀ� ȣ��Ǿ� �ٸ� ��忡�� ������ �� �������մϴ�.
	/// �� �޼���� �׷��� ������Ʈ �߿��� ȣ��Ǿ�� �մϴ�. ��, ��� ã�� �����尡 ���� ������ �ʰų� �Ͻ� ������ ��쿡�� ȣ��Ǿ�� �մϴ�.
	///
	/// ���: ����� �ڵ忡�� �� �޼��带 ���� ȣ���ؼ��� �ȵ˴ϴ�. ���������� �ý��ۿ��� ���˴ϴ�.
	/// </summary>
	internal void DestroyNode (GraphNode node) {
		pathProcessor.DestroyNode(node);
	}

	/// <summary>
	/// ��� ��� ã�� �����尡 �Ͻ� �����ǰ� ���� �� ������ �����մϴ�.
	///
	/// <code>
	/// var graphLock = AstarPath.active.PausePathfinding();
	/// // ���⿡�� �׷����� �����ϰ� ������ �� �ֽ��ϴ�. ���� ��� ����Ʈ �׷����� ���ο� ��带 �߰��մϴ�.
	/// var node = AstarPath.active.data.pointGraph.AddNode((Int3) new Vector3(3, 1, 4));
	///
	/// // ��� ã�⸦ �簳�Ϸ���
	/// graphLock.Release();
	/// </code>
	///
	/// ��ȯ: ��� ��ü�Դϴ�. ��� ã�⸦ �ٽ� �����Ϸ��� <see cref="Pathfinding.PathProcessor.GraphUpdateLock.Release"/>�� ȣ���ؾ� �մϴ�.
	/// ����: ��κ��� ��� ����� �ڵ忡�� ���� ȣ���ؼ��� �ȵ˴ϴ�. ��� <see cref="AddWorkItem"/> �޼��带 ����Ͻʽÿ�.
	///
	/// ����: <see cref="AddWorkItem"/>
	/// </summary>
	public PathProcessor.GraphUpdateLock PausePathfinding () {
		return pathProcessor.PausePathfinding(true);
	}

	/// <summary>��� ť�� �����Ͽ� �۾� �׸��� �����մϴ�.</summary>
	PathProcessor.GraphUpdateLock PausePathfindingSoon () {
		return pathProcessor.PausePathfinding(false);
	}

	/// <summary>
	/// Ư�� �׷����� ��ĵ�մϴ�.
	/// �� �޼��带 ȣ���ϸ� ������ �׷����� �ٽ� ����մϴ�.
	/// �� �޼���� ����� ���� �� �����Ƿ� (�׷��� ���� �� �׷��� ���⼺�� ���� �ٸ��ϴ�) ������ ���� �׷��� ������Ʈ�� ����ϴ� ���� �����ϴ�.
	///
	/// <code>
	/// // ��� �׷��� �ٽ� ���
	/// AstarPath.active.Scan();
	///
	/// // ù ��° �׸��� �׷����� �ٽ� ���
	/// var graphToScan = AstarPath.active.data.gridGraph;
	/// AstarPath.active.Scan(graphToScan);
	///
	/// // ù ��° �� �� ��° �׷����� �ٽ� ���
	/// var graphsToScan = new [] { AstarPath.active.data.graphs[0], AstarPath.active.data.graphs[2] };
	/// AstarPath.active.Scan(graphsToScan);
	/// </code>
	///
	/// ����: �׷��� ������Ʈ (�¶��� �������� �۵� ��ũ Ȯ��)
	/// ����: ScanAsync
	/// </summary>
	public void Scan (NavGraph graphToScan) {
		if (graphToScan == null) throw new System.ArgumentNullException();
		Scan(new NavGraph[] { graphToScan });
	}

	/// <summary>
	/// ������ ��� �׷����� ��ĵ�մϴ�.
	///
	/// �� �޼��带 ȣ���ϸ� ������ ��� �׷��� �Ǵ� graphsToScan �Ű������� null�� ��� ��� �׷����� �ٽ� ����մϴ�.
	/// �� �޼���� ����� ���� �� �����Ƿ� (�׷��� ���� �� �׷��� ���⼺�� ���� �ٸ��ϴ�) ������ ���� �׷��� ������Ʈ�� ����ϴ� ���� �����ϴ�.
	///
	/// <code>
	/// // ��� �׷��� �ٽ� ���
	/// AstarPath.active.Scan();
	///
	/// // ù ��° �׸��� �׷����� �ٽ� ���
	/// var graphToScan = AstarPath.active.data.gridGraph;
	/// AstarPath.active.Scan(graphToScan);
	///
	/// // ù ��° �� �� ��° �׷����� �ٽ� ���
	/// var graphsToScan = new [] { AstarPath.active.data.graphs[0], AstarPath.active.data.graphs[2] };
	/// AstarPath.active.Scan(graphsToScan);
	/// </code>
	///
	/// ����: �׷��� ������Ʈ (�¶��� �������� �۵� ��ũ Ȯ��)
	/// ����: ScanAsync
	/// </summary>
	/// <param name="graphsToScan">��ĵ�� �׷����Դϴ�. �� �Ű������� null�� ��� ��� �׷����� ��ĵ�˴ϴ�.</param>
	public void Scan (NavGraph[] graphsToScan = null) {
		var prevProgress = new Progress();

		Profiler.BeginSample("Scan");
		Profiler.BeginSample("Init");
		foreach (var p in ScanAsync(graphsToScan)) {
			if (prevProgress.description != p.description) {
#if !NETFX_CORE && UNITY_EDITOR
				Profiler.EndSample();
				Profiler.BeginSample(p.description);
				// Log progress to the console
				System.Console.WriteLine(p.description);
				prevProgress = p;
#endif
			}
		}
		Profiler.EndSample();
		Profiler.EndSample();
	}

	/// <summary>
	/// Ư�� �׷����� �񵿱�� ��ĵ�մϴ�. �̴� IEnumerable�̹Ƿ� ������� �������� ������ ����� �� �ֽ��ϴ�.
	/// <code>
	/// foreach (Progress progress in AstarPath.active.ScanAsync()) {
	///     Debug.Log("Scanning... " + progress.description + " - " + (progress.progress*100).ToString("0") + "%");
	/// }
	/// </code>
	/// ������� ����� �� �񵿱�� �׷����� ��ĵ�� �� �ֽ��ϴ�.
	/// �̰��� ���� ������ �ӵ��� ���������� ������ ��ĵ �߿� ����� �ٸ� ǥ���� �� �ְ� �մϴ�.
	/// <code>
	/// IEnumerator Start () {
	///     foreach (Progress progress in AstarPath.active.ScanAsync()) {
	///         Debug.Log("Scanning... " + progress.description + " - " + (progress.progress*100).ToString("0") + "%");
	///         yield return null;
	///     }
	/// }
	/// </code>
	///
	/// ����: Scan
	/// </summary>
	public IEnumerable<Progress> ScanAsync (NavGraph graphToScan) {
		if (graphToScan == null) throw new System.ArgumentNullException();
		return ScanAsync(new NavGraph[] { graphToScan });
	}

	/// <summary>
	/// ������ ��� �׷����� �񵿱�� ��ĵ�մϴ�. �̴� IEnumerable�̹Ƿ� ������� �������� ������ ����� �� �ֽ��ϴ�.
	///
	/// <code>
	/// foreach (Progress progress in AstarPath.active.ScanAsync()) {
	///     Debug.Log("Scanning... " + progress.description + " - " + (progress.progress*100).ToString("0") + "%");
	/// }
	/// </code>
	/// ������� ����� �� �񵿱�� �׷����� ��ĵ�� �� �ֽ��ϴ�.
	/// �̰��� ���� ������ �ӵ��� ���������� ������ ��ĵ �߿� ����� �ٸ� ǥ���� �� �ְ� �մϴ�.
	/// <code>
	/// IEnumerator Start () {
	///     foreach (Progress progress in AstarPath.active.ScanAsync()) {
	///         Debug.Log("Scanning... " + progress.description + " - " + (progress.progress*100).ToString("0") + "%");
	///         yield return null;
	///     }
	/// }
	/// </code>
	///
	/// ����: Scan
	/// </summary>
	/// <param name="graphsToScan">��ĵ�� �׷����Դϴ�. �� �Ű������� null�� ��� ��� �׷����� ��ĵ�˴ϴ�.</param>
	public IEnumerable<Progress> ScanAsync (NavGraph[] graphsToScan = null) {
		if (graphsToScan == null) graphsToScan = graphs;

		if (graphsToScan == null) {
			yield break;
		}

		if (isScanning) throw new System.InvalidOperationException("�ٸ� �񵿱� ��ĵ�� �̹� ���� ���Դϴ�");

		isScanning = true;

		VerifyIntegrity();

		var graphUpdateLock = PausePathfinding();

		// ť�� ��ȯ �� ��� ��ΰ� ��� ��ȯ�ǵ��� �մϴ�
		// �Ϻ� ����� (��: funnel �����)�� ��ΰ� ��ȯ �� �� ��尡 ��ȿ�� ������ �����ϱ� �����Դϴ�.
		pathReturnQueue.ReturnPaths(false);

		if (!Application.isPlaying) {
			data.FindGraphTypes();
			GraphModifier.FindAllModifiers();
		}

		int startFrame = Time.frameCount;

		yield return new Progress(0.05F, "Pre processing graphs");

		// ��,�� ������ �����ϱ� ����
		// �ڵ�� ���� ������ ���� ������ ���� ������ �ڵ带 ���� ���� �������ϴ�.
		// ���⼭ ������ �� �� �ִ��� ��� �ֽø� �����ϰڽ��ϴ�.
		// A * ��� ã�� ������Ʈ�� ���� ������ ������ �ֽñ� �ٶ��ϴ�.
		if (Time.frameCount != startFrame) {
			throw new System.Exception("�񵿱� ��ĵ�� A * ��� ã�� ������Ʈ�� ���� ���������� ������ �� �ֽ��ϴ�.");
		}

		if (OnPreScan != null) {
			OnPreScan(this);
		}

		GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);

		data.LockGraphStructure();

		Physics2D.SyncTransforms();
		var watch = System.Diagnostics.Stopwatch.StartNew();

		// ���� ��带 �ı��մϴ�.
		for (int i = 0; i < graphsToScan.Length; i++) {
			if (graphsToScan[i] != null) {
				((IGraphInternals)graphsToScan[i]).DestroyAllNodes();
			}
		}

		// �׷����� �ϳ��� ������ ���� ��ĵ�մϴ�.
		for (int i = 0; i < graphsToScan.Length; i++) {
			// �� �׷��� �ǳ� �ݴϴ�.
			if (graphsToScan[i] == null) continue;

			// ���� ���������� ��
			// �� �׷����� ���� ���븦 minp���� maxp�� �̵���ŵ�ϴ�.
			float minp = Mathf.Lerp(0.1F, 0.8F, (float)(i)/(graphsToScan.Length));
			float maxp = Mathf.Lerp(0.1F, 0.8F, (float)(i+0.95F)/(graphsToScan.Length));

			var progressDescriptionPrefix = "Scanning graph " + (i+1) + " of " + graphsToScan.Length + " - ";

			// ���� ó�� ������ �ణ ���������� ���� ó�� ������ foreach ������ ����������
			// (try-except �� ������ yield �� �� ����) ���� ó���Դϴ�.
			var coroutine = ScanGraph(graphsToScan[i]).GetEnumerator();
			while (true) {
				try {
					if (!coroutine.MoveNext()) break;
				} catch {
					isScanning = false;
					data.UnlockGraphStructure();
					graphUpdateLock.Release();
					throw;
				}
				yield return coroutine.Current.MapTo(minp, maxp, progressDescriptionPrefix);
			}
		}

		data.UnlockGraphStructure();
		yield return new Progress(0.8F, "Post processing graphs");

		if (OnPostScan != null) {
			OnPostScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);

		FlushWorkItems();

		yield return new Progress(0.9F, "Computing areas");

		hierarchicalGraph.RecalculateIfNecessary();

		yield return new Progress(0.95F, "Late post processing");

		// ���⿡�� ��ĵ�� ���� �� ���� ��ȣ�� �����ϴ�
		// �� ���� ���Ŀ��� � ���͵� �Ͼ���� �ȵ˴ϴ�.
		// �ٸ� �ý����� �Ϻΰ� �����ϱ� �����ϱ� �����Դϴ�.
		isScanning = false;

		if (OnLatePostScan != null) {
			OnLatePostScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);

		euclideanEmbedding.dirty = true;
		euclideanEmbedding.RecalculatePivots();

		// ���� �۾� ����
		FlushWorkItems();
		// ��� ã�� ������ �簳
		graphUpdateLock.Release();

		watch.Stop();
		lastScanTime = (float)watch.Elapsed.TotalSeconds;

		System.GC.Collect();

		if (logPathResults != PathLog.None && logPathResults != PathLog.OnlyErrors) {
			Debug.Log("Scanning - Process took "+(lastScanTime*1000).ToString("0")+" ms to complete");
		}
	}

	IEnumerable<Progress> ScanGraph (NavGraph graph) {
		if (OnGraphPreScan != null) {
			yield return new Progress(0, "Pre processing");
			OnGraphPreScan(graph);
		}

		yield return new Progress(0, "");

		foreach (var p in ((IGraphInternals)graph).ScanInternal()) {
			yield return p.MapTo(0, 0.95f);
		}

		yield return new Progress(0.95f, "Assigning graph indices");

		// �׷��� �� ��� ��忡 �׷��� �ε��� �Ҵ�
		graph.GetNodes(node => node.GraphIndex = (uint)graph.graphIndex);

		if (OnGraphPostScan != null) {
			yield return new Progress(0.99f, "Post processing");
			OnGraphPostScan(graph);
		}
	}

	#endregion

	private static int waitForPathDepth = 0;

	/// <summary>
	/// ��ΰ� ���� ������ ���ܵ˴ϴ�.
	///
	/// �Ϲ������� ��ΰ� ���ǰ� ��ȯ�Ǳ���� �� �������� �ҿ�˴ϴ�.
	/// �� �Լ��� �Լ��� ��ȯ�Ǹ� ��ΰ� ���ǰ� �ش� ��ο� ���� �ݹ��� ȣ��ǵ��� �����մϴ�.
	///
	/// ���� ��θ� �� ���� ��û�ϰ� ������ ��ΰ� �Ϸ�� ������ ����ϴ� ���,
	/// ť�� �ִ� ��� ��κ��� ����մϴ� (��Ƽ�������� ����ϴ� ��� ��κи� ���Ǹ�,
	/// ��Ƽ�������� ������� �ʴ� ��� ��� ���˴ϴ�).
	///
	/// �� �Լ��� ������ �ʿ��� ��쿡�� ����Ͻʽÿ�.
	/// ��� ����� ���� �����ӿ� ���� �л��ϴ� ���� �����ϴ�.
	/// �̷��� �ϸ� �����ӷ��� �ε巴�� �����ϰ� ���ÿ� ���� ��θ� ���ÿ� ��û�ص� ���� �߻����� �ʽ��ϴ�.
	///
	/// ����: �� �Լ��� ���� �߿� �׷��� ������Ʈ �� ��Ÿ �ݹ��� ȣ��� �� �ֽ��ϴ�.
	///
	/// �н����δ��� ���� ���� ���. ��, OnDestroy������ �� �Լ��� �ƹ� �۾��� �������� �ʽ��ϴ�.
	///
	/// \throws Exception ��θ� ����ϴ� ���� ��� ã�Ⱑ �� ��鿡�� �ùٸ��� �ʱ�ȭ���� �ʾҰų� (�Ƹ��� AstarPath ��ü�� ����)
	/// �Ǵ� ��ΰ� ���� ���۵��� �ʾ��� �� ���ܰ� �߻��մϴ�.
	/// �Ϲ����� ��� �߻����� �ʵ��� ������ε� �����尡 �浹�� ���� ���� �ɰ��� ������ �߻��ϸ� ���ܰ� �߻��մϴ� (���� �Ϲ����� ��쿡�� �߻����� �ʾƾ� �մϴ�).
	/// ��� ��� �� ���� ������ �����ϱ� ���� ��ġ�Դϴ�.
	///
	/// ����: Pathfinding.Path.WaitForPath
	/// ����: Pathfinding.Path.BlockUntilCalculated
	/// </summary>
	/// <param name="path">����� ���. ��ΰ� ���۵��� �ʾҴٸ� ���ܰ� �߻��մϴ�.</param>
	public static void BlockUntilCalculated (Path path) {
		if (active == null)
			throw new System.Exception("�� ��鿡�� ��� ã�Ⱑ �ùٸ��� �ʱ�ȭ���� �ʾҽ��ϴ�. " +
		   "AstarPath.active�� null�Դϴ�.\nAwake���� �� �Լ��� ȣ������ ���ʽÿ�.");

		if (path == null) throw new System.ArgumentNullException("��δ� null�� �� �� �����ϴ�");

		if (active.pathProcessor.queue.IsTerminating) return;

		if (path.PipelineState == PathState.Created)
		{
			throw new System.Exception("������ ��ΰ� ���� ���۵��� �ʾҽ��ϴ�.");
		}

		waitForPathDepth++;

		if (waitForPathDepth == 5)
		{
			Debug.LogError("��������� BlockUntilCalculated �Լ��� ȣ���ϰ� �ֽ��ϴ� (�Ƹ��� ��� �ݹ鿡��). �̷��� ���� ���ʽÿ�.");
		}

		if (path.PipelineState < PathState.ReturnQueue) {
			if (active.IsUsingMultithreading) {
				while (path.PipelineState < PathState.ReturnQueue) {
					if (active.pathProcessor.queue.IsTerminating)
					{
						waitForPathDepth--;
						throw new System.Exception("�н����ε� �����尡 �浹�� ������ ���Դϴ�.");
					}

					// �����尡 ��θ� ����ϱ⸦ ��ٸ��ϴ�
					Thread.Sleep(1);
					active.PerformBlockingActions(true);
				}
			} else {
				while (path.PipelineState < PathState.ReturnQueue) {
					if (active.pathProcessor.queue.IsEmpty && path.PipelineState != PathState.Processing)
					{
						waitForPathDepth--;
						throw new System.Exception("�ɰ��� �����Դϴ�. ��� ť�� ��� ������ ��� ���´� '" + path.PipelineState + "'�Դϴ�.");
					}

					// �Ϻ� ��� ���
					active.pathProcessor.TickNonMultithreaded();
					active.PerformBlockingActions(true);
				}
			}
		}

		active.pathReturnQueue.ReturnPaths(false);
		waitForPathDepth--;
	}

	/// <summary>
	/// ��θ� ������ ���� ���ǵ��� ��⿭�� �߰��մϴ�.
	/// ��ΰ� ���� �� ������ �ݹ��� ȣ��˴ϴ�.
	/// �Ϲ������� ���� �� �Լ��� ȣ���ϴ� ��� Seeker ������Ʈ�� ����ؾ� �մϴ�.
	/// </summary>
	/// <param name="path">��⿭�� �߰��� ����Դϴ�.</param>
	/// <param name="pushToFront">true�� ��� ��ΰ� ��⿭�� �������� Ǫ�õǾ� ��� ���� �ٸ� ��θ� ��ȸ�ϰ� ������ ���˴ϴ�.
	/// �̰��� �ٸ� ��κ��� �켱�Ͽ� ����Ϸ��� ��ΰ� ���� �� ������ �� �ֽ��ϴ�. �׷��� �����ϰ� ����ϸ� �����ؾ� �մϴ�.
	/// �ʹ� ���� ��ΰ� ���� ���ʿ� Ǫ�õǸ� ��ΰ� ���Ǳ���� �ٸ� ��ΰ� ���� ���� �ð��� ��ٸ� �� �ֽ��ϴ�.</param>
	public static void StartPath (Path path, bool pushToFront = false) {
		// ���߽����� ������ ���ϱ� ���� ���� ������ �����մϴ�.
		var astar = active;

		if (System.Object.ReferenceEquals(astar, null))
		{
			Debug.LogError("�� ��鿡�� AstarPath ��ü�� ���ų� ���� �ʱ�ȭ���� �ʾҽ��ϴ�.");
			return;
		}

		if (path.PipelineState != PathState.Created)
		{
			throw new System.Exception("����� ���°� �߸��Ǿ����ϴ�. " + PathState.Created + " ���°� ����Ǿ����� " + path.PipelineState + " ���¸� ã�ҽ��ϴ�.\n" +
				"������ ��θ� �� �� ��û���� �ʵ��� Ȯ���Ͻʽÿ�.");
		}

		if (astar.pathProcessor.queue.IsTerminating)
		{
			path.FailWithError("�� ��δ� ������ �ʽ��ϴ�");
			return;
		}

		if (astar.graphs == null || astar.graphs.Length == 0)
		{
			Debug.LogError("��鿡 �׷����� �����ϴ�");
			path.FailWithError("��鿡 �׷����� �����ϴ�");
			Debug.LogError(path.errorLog);
			return;
		}

		path.Claim(astar);

		// ���¸� PathState.PathQueue�� ������ŵ�ϴ�
		((IPathInternals)path).AdvanceState(PathState.PathQueue);
		if (pushToFront) {
			astar.pathProcessor.queue.PushFront(path);
		} else {
			astar.pathProcessor.queue.Push(path);
		}

		// �÷��� ��� �ܺο����� ��� ��� ��û�� �������Դϴ�
		if (!Application.isPlaying) {
			BlockUntilCalculated(path);
		}
	}

	/// <summary>
	/// ���ʿ��� �Ҵ��� ���ϱ� ���� NNConstraint.None�� ĳ���մϴ�.
	/// �� ������ NNConstraint�� �Һ� Ŭ����/����ü�� ���� �̻������� �����ؾ� �մϴ�.
	/// </summary>
	static readonly NNConstraint NNConstraintNone = NNConstraint.None;

	/// <summary>
	/// ��ġ�� ���� ����� ��带 ��ȯ�մϴ�.
	/// �� �޼���� ��� �׷����� �˻��Ͽ� �ش� ��ġ�� ���� ����� ��带 �����ϰ� ��ȯ�մϴ�.
	///
	/// GetNearest(position, NNConstraint.None)�� �����մϴ�.
	///
	/// <code>
	/// // �� ���� ������Ʈ ��ġ�� ���� ����� ��带 ã���ϴ�.
	/// GraphNode node = AstarPath.active.GetNearest(transform.position).node;
	///
	/// if (node.Walkable) {
	///     // ��尡 �ȱ� �����ϸ� ���⿡ Ÿ���� ��ġ�ϰų� ��Ÿ �۾��� ������ �� �ֽ��ϴ�.
	/// }
	/// </code>
	///
	/// ����: Pathfinding.NNConstraint
	/// </summary>
	public NNInfo GetNearest (Vector3 position) {
		return GetNearest(position, NNConstraintNone);
	}

	/// <summary>
	/// ������ NNConstraint�� ����Ͽ� ��ġ�� ���� ����� ��带 ��ȯ�մϴ�.
	/// ��� �׷����� �˻��Ͽ� ������ ��ġ�� ���� ����� ��带 �����մϴ�.
	/// NNConstraint�� �ȱ� ������ ��常 �����ϴ� �� � ��带 �������� ������ �����ϴ� �� ����� �� �ֽ��ϴ�.
	///
	/// <code>
	/// GraphNode node = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
	/// </code>
	///
	/// <code>
	/// var constraint = NNConstraint.None;
	///
	/// // �ȱ� ������ ��常 �˻����� ���� ����
	/// constraint.constrainWalkability = true;
	/// constraint.walkable = true;
	///
	/// // �±� 3 �Ǵ� �±� 5�� ��常 �˻����� ���� ����
	/// // 'tags' �ʵ�� ��Ʈ����ũ�Դϴ�.
	/// constraint.constrainTags = true;
	/// constraint.tags = (1 << 3) | (1 << 5);
	///
	/// var info = AstarPath.active.GetNearest(transform.position, constraint);
	/// var node = info.node;
	/// var closestPoint = info.position;
	/// </code>
	///
	/// ����: Pathfinding.NNConstraint
	/// </summary>
	public NNInfo GetNearest (Vector3 position, NNConstraint constraint) {
		return GetNearest(position, constraint, null);
	}

	/// <summary>
	/// ������ NNConstraint�� ����Ͽ� ��ġ�� ���� ����� ��带 ��ȯ�մϴ�.
	/// ��� �׷����� �˻��Ͽ� ������ ��ġ�� ���� ����� ��带 �����մϴ�.
	/// NNConstraint�� �ȱ� ������ ��常 �����ϴ� �� � ��带 �������� ������ �����ϴ� �� ����� �� �ֽ��ϴ�.
	/// ����: Pathfinding.NNConstraint
	/// </summary>
	public NNInfo GetNearest (Vector3 position, NNConstraint constraint, GraphNode hint) {
		// �Ӽ� ��ȸ�� ĳ���մϴ�.
		var graphs = this.graphs;

		float minDist = float.PositiveInfinity;
		NNInfoInternal nearestNode = new NNInfoInternal();
		int nearestGraph = -1;

		if (graphs != null) {
			for (int i = 0; i < graphs.Length; i++) {
				NavGraph graph = graphs[i];

				// �� �׷����� �˻��ؾ� �ϴ��� Ȯ���մϴ�.
				if (graph == null || !constraint.SuitableGraph(i, graph)) {
					continue;
				}

				NNInfoInternal nnInfo;
				if (fullGetNearestSearch) {
					// ���� ���� ����� ��� �˻�
					// �̴� ���࿡ ���� ������ ��带 ã������ �õ��մϴ�.
					nnInfo = graph.GetNearestForce(position, constraint);
				} else {
					// ���� ���� ����� ��� �˻�
					// ������ ũ�� ������� �ʰ� ��ġ�� ����� ��带 ã���ϴ�.
					nnInfo = graph.GetNearest(position, constraint);
				}

				GraphNode node = nnInfo.node;

				// �� �׷������� ��带 ã�� ���� ���
				if (node == null) {
					continue;
				}

				// ��û�� ��ġ���� ����� ���� ����� �������� �Ÿ�
				float dist = ((Vector3)nnInfo.clampedPosition-position).magnitude;

				if (prioritizeGraphs && dist < prioritizeGraphsLimit) {
					// ��尡 ����� ������ �� �׷����� �����ϰ� �ٸ� ���� ��� �����մϴ�.
					minDist = dist;
					nearestNode = nnInfo;
					nearestGraph = i;
					break;
				} else {
					// ���ݱ��� ã�� ������ ��带 �����մϴ�.
					if (dist < minDist) {
						minDist = dist;
						nearestNode = nnInfo;
						nearestGraph = i;
					}
				}
			}
		}

		// ��ġ�ϴ� �׸��� ã�� ���� ���
		if (nearestGraph == -1) {
			return new NNInfo();
		}

		// �̹� ���� ��尡 �����Ǿ����� Ȯ���մϴ�.
		if (nearestNode.constrainedNode != null) {
			nearestNode.node = nearestNode.constrainedNode;
			nearestNode.clampedPosition = nearestNode.constClampedPosition;
		}

		if (!fullGetNearestSearch && nearestNode.node != null && !constraint.Suitable(nearestNode.node)) {
			// �׷��� ������, �׷����� ������ ��带 Ȯ���ϵ��� ������ �˻��մϴ�.
			NNInfoInternal nnInfo = graphs[nearestGraph].GetNearestForce(position, constraint);

			if (nnInfo.node != null) {
				nearestNode = nnInfo;
			}
		}

		if (!constraint.Suitable(nearestNode.node) || (constraint.constrainDistance && (nearestNode.clampedPosition - position).sqrMagnitude > maxNearestNodeDistanceSqr)) {
			return new NNInfo();
		}

		// ���� �ʵ尡 ��� �ִ� NNInfo�� ��ȯ�մϴ�.
		return new NNInfo(nearestNode);
	}

	/// <summary>
	/// ���̿� ���� ����� ��带 ��ȯ�մϴ� (�����ϴ�).
	/// ���: �� �Լ��� ������ �������̸� �ſ� ���� �� �����Ƿ� �����ؼ� ����ϼ���.
	/// </summary>
	public GraphNode GetNearest (Ray ray) {
		if (graphs == null) return null;

		float minDist = Mathf.Infinity;
		GraphNode nearestNode = null;

		Vector3 lineDirection = ray.direction;
		Vector3 lineOrigin = ray.origin;

		for (int i = 0; i < graphs.Length; i++) {
			NavGraph graph = graphs[i];

			graph.GetNodes(node => {
				Vector3 pos = (Vector3)node.position;
				Vector3 p = lineOrigin+(Vector3.Dot(pos-lineOrigin, lineDirection)*lineDirection);

				float tmp = Mathf.Abs(p.x-pos.x);
				tmp *= tmp;
				if (tmp > minDist) return;

				tmp = Mathf.Abs(p.z-pos.z);
				tmp *= tmp;
				if (tmp > minDist) return;

				float dist = (p-pos).sqrMagnitude;

				if (dist < minDist) {
					minDist = dist;
					nearestNode = node;
				}
				return;
			});
		}

		return nearestNode;
	}
}
