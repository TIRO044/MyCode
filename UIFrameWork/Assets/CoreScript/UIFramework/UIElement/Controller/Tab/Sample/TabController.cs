using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoreScript.UIFramework.UIElement.Controller.Tab.Sample
{
    public class TabsSample : MonoBehaviour
    {
        // grid ó���� �˾Ƽ� �ʿ��ϱ� �ϰ���?
        private List<TabSample> _tabs;

        public GridLayoutGroup _gridLayoutGroup;


        public void Awake()
        {
            _tabs = new List<TabSample>(GetComponents<TabSample>());
            
            foreach (var tabSample in _tabs)
            {
                tabSample.RegisterOnPointClick(OnClickTab);
            }

            // �䷸�� ���� ����, �׳� �޼��� ������� �ϸ� �� ���� ���� �� ������
        }

        public void OnClickTab(TabSample tabManager)
        {

        }
    }
}