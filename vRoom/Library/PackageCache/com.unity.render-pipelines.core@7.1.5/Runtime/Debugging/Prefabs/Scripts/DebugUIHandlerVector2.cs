using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
    public class DebugUIHandlerVector2 : DebugUIHandlerWidget
    {
        public Text nameLabel;
        public UIFoldout valueToggle;

        public DebugUIHandlerIndirectFloatField fieldX;
        public DebugUIHandlerIndirectFloatField fieldY;

        DebugUI.Vector2Field m_Field;
        DebugUIHandlerContainer m_Container;

        internal override void SetWidget(DebugUI.Widget widget)
        {
            base.SetWidget(widget);
            m_Field = CastWidget<DebugUI.Vector2Field>();
            m_Container = GetComponent<DebugUIHandlerContainer>();
            nameLabel.text = m_Field.displayName;

            fieldX.getter = () => m_Field.GetValue().x;
            fieldX.setter = x => SetValue(x, x: true);
            fieldX.nextUIHandler = fieldY;
            SetupSettings(fieldX);

            fieldY.getter = () => m_Field.GetValue().y;
            fieldY.setter = x => SetValue(x, y: true);
            fieldY.previousUIHandler = fieldX;
            SetupSettings(fieldY);
        }

        void SetValue(float v, bool x = false, bool y = false)
        {
            var vec = m_Field.GetValue();
            if (x) vec.x = v;
            if (y) vec.y = v;
            m_Field.SetValue(vec);
        }

        void SetupSettings(DebugUIHandlerIndirectFloatField field)
        {
            field.parentUIHandler = this;
            field.incStepGetter = () => m_Field.incStep;
            field.incStepMultGetter = () => m_Field.incStepMult;
            field.decimalsGetter = () => m_Field.decimals;
            field.Init();
        }

        public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
        {
            if (fromNext || valueToggle.isOn == false)
            {
                nameLabel.color = colorSelected;
            }
            else if (valueToggle.isOn)
            {
                if (m_Container.IsDirectChild(previous))
                {
                    nameLabel.color = colorSelected;
                }
                else
                {
                    var lastItem = m_Container.GetLastItem();
                    DebugManager.instance.ChangeSelection(lastItem, false);
                }
            }

            return true;
        }

        public override void OnDeselection()
        {
            nameLabel.color = colorDefault;
        }

        public override void OnIncrement(bool fast)
        {
            valueToggle.isOn = true;
        }

        public override void OnDecrement(bool fast)
        {
            valueToggle.isOn = false;
        }

        public override void OnAction()
        {
            valueToggle.isOn = !valueToggle.isOn;
        }

        public override DebugUIHandlerWidget Next()
        {
            if (!valueToggle.isOn || m_Container == null)
                return base.Next();

            var firstChild = m_Container.GetFirstItem();

            if (firstChild == null)
                return base.Next();

            return firstChild;
        }
    }
}
