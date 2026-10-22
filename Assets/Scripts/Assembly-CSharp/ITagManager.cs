public interface ITagManager
{
	void LoadContainerDefaultOnly(string containerId, int defaultContainerResourceId);

	void LoadContainerPreferFresh(string containerId, int defaultContainerResourceId);

	void LoadContainerPreferNonDefault(string containerId, int defaultContainerResourceId);
}
