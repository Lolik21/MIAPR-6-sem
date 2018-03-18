using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perseptron
{
    public class PerseptronAlgorithm
    {

        private const int RandomScalar = 10;
        private const int RandomDevider = RandomScalar / 2;
        private const int ExpandedValue = 1;

        private List<int> Decisions { get; set; }


        public PerseptronAlgorithm()
        {
            Classes = new List<PerceptronClass>();
            Weights = new List<PerceptronObject>();
            Decisions = new List<int>();
        }

        public List<PerceptronClass> Classes { get; set; }

        public List<PerceptronObject> Weights { get; set; }

        public int FindClass(PerceptronObject perceptronObject)
        {
            int resultClass = 0;
            int decisionMax;

            perceptronObject.Attribues.Add(1);
            decisionMax = int.MinValue;

            for (int i = 0; i < Classes.Count; i++)
            {
                for (int j = 0; j < Classes[i].Objects.Count; j++)
                {
                    var isEquals = true;
                    var list = Classes[i].Objects[j].Attribues;
                    for (int k = 0; k < perceptronObject.Attribues.Count; k++)
                    {
                        if (list[k] != perceptronObject.Attribues[k])
                        {
                            isEquals = false;
                        }
                    }

                    if (isEquals) return i + 1;
                }
            }

            for (int i = 0; i < Weights.Count; i++)
            {
                PerceptronObject weigth = Weights[i];

                if (ObjectMultiplication(weigth, perceptronObject) > decisionMax)
                {
                    decisionMax = ObjectMultiplication(weigth, perceptronObject);
                    resultClass = i;
                }
            }

            return (resultClass + 1);
        }

        public void Calculate(int classesCount,
            int objectsCount, int attributesCount)
        {
            Init(classesCount, objectsCount, attributesCount);
            DoCalculation();
        }

        private void DoCalculation()
        {
            bool IsDone = false;
            int k = 0;
            while (!IsDone)
            {
                for (int i = 0; i < Classes.Count; i++)
                {
                    PerceptronClass selectedClass = Classes[i];
                    PerceptronObject selectedWeight = Weights[i];
                    CalculateWeightCorrection(selectedClass,
                        selectedWeight, ref IsDone, i);
                }
                k++;
                if (k > 1000) IsDone = true;
            }
        }

        private void CalculateWeightCorrection(PerceptronClass selectedClass,
            PerceptronObject selectedWeight, ref bool IsDone, int classId)
        {
            for (int i = 0; i < selectedClass.Objects.Count; i++)
            {
                PerceptronObject selectedObject = selectedClass.Objects[i];
                CorrectWeight(selectedObject, selectedWeight,ref IsDone, classId);
            }
        }

        private void CorrectWeight(PerceptronObject selectedObject,
            PerceptronObject selectedWeight, ref bool IsDone, int ClassId)
        {
            bool result = false;
            int objectDecision = ObjectMultiplication(selectedWeight, selectedObject);

            for (int i = 0; i < Weights.Count; i++)
            {
                Decisions[i] = ObjectMultiplication(Weights[i], selectedObject);

                if (i != ClassId)
                {
                    int currentDecision = Decisions[i];
                    if (objectDecision <= currentDecision)
                    {
                        ChangeWeigth(Weights[i], selectedObject, -1);
                        result = true;
                    }
                }
            }
            if (result)
                ChangeWeigth(selectedWeight, selectedObject, 1);

            IsDone = result;
        }

        private void ChangeWeigth(PerceptronObject weigth, PerceptronObject perceptronObject, int sign)
        {
            for (int i = 0; i < weigth.Attribues.Count; i++)
                weigth.Attribues[i] += sign * perceptronObject.Attribues[i];
        }

        private int ObjectMultiplication(PerceptronObject weigth, PerceptronObject obj)
        {
            int result = 0;

            for (int i = 0; i < weigth.Attribues.Count; i++)
                result += weigth.Attribues[i] * obj.Attribues[i];

            return result;
        }

        private void Init(int classesCount,
            int objectsCount, int attributesCount)
        {
            Clear();
            FillClassesWithRandomAttributes(classesCount,
                objectsCount, attributesCount);
            FillWeights(attributesCount);
            FillDecisions();
            ExpandAttributes();
        }

        private void ExpandAttributes()
        {
            Classes = Classes.Select(pclass =>
            {
                pclass.Objects = pclass.Objects.Select(obj =>
                {
                    obj.Attribues.Add(ExpandedValue);
                    return obj;
                }).ToList();
                return pclass;
            }).ToList();
        }

        private void Clear()
        {
            Classes.Clear();
            Weights.Clear();
        }

        private void FillDecisions()
        {
            foreach (var pclass in Classes)
            {
                Decisions.Add(0);
            }
        }

        private void FillWeights(int attributesCount)
        {
            foreach(var pclass in Classes)
            {
                Weights.Add(new PerceptronObject());
                Weights.Last().Attribues = new List<int>();
                for (int i = 0; i <= attributesCount; i++)
                {
                    Weights.Last().Attribues.Add(0);
                }
            }
        }

        private void FillClassesWithRandomAttributes(int classesCount,
            int objectsCount, int attributesCount)
        {
            Random random = new Random();
            for (int i = 0; i < classesCount; i++)
            {
                Classes.Add(new PerceptronClass());
                for (int j = 0; j < objectsCount; j++)
                {
                    Classes[i].Objects.Add(new PerceptronObject());
                    for (int k = 0; k < attributesCount; k++)
                    {
                        Classes[i].Objects[j].Attribues
                            .Add(random.Next(RandomScalar) - RandomDevider);

                    }
                }
            }
        }


        public class PerceptronClass
        {
            public PerceptronClass()
            {
                Objects = new List<PerceptronObject>();
            }
            public List<PerceptronObject> Objects { get; set; }
        }

        public class PerceptronObject
        {
            public PerceptronObject()
            {
                Attribues = new List<int>();
            }
                    
            public List<int> Attribues { get; set; }
        }
    }
}

