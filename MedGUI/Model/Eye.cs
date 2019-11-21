using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model
{
    public class Eye: IComparable
    {
        public int Id { get; set; }
        public int VMPId { get; set; }
        public virtual VMP VMP { get; set; }
        public int EyeSideId { get; set; }
        public virtual EyeSide EyeSide { get; set; }
        public string Visus { get; set; }
        public string EyeBallPosition { get; set; }
        public string EyeBallPositionDetail { get; set; }
        public string Motion { get; set; }
        public string MotionDetail { get; set; }
        public string Character { get; set; }
        public string CharacterDetail { get; set; }
        public string EyeLids { get; set; }
        public string EyeLidsDetail { get; set; }
        public string EyeLashes { get; set; }
        public string EyeLashesDetail { get; set; }
        public string Palpation { get; set; }
        public string PalpationDetail { get; set; }
        public string Conjunctiva { get; set; }
        public string ConjunctivaDetail { get; set; }
        public string Cornea { get; set; }
        public string CorneaDetail { get; set; }
        public string FrontCamera { get; set; }
        public string Moisture { get; set; }
        public string MoistureDetail { get; set; }
        public string FrontCameraAngle { get; set; }
        public string Iris { get; set; }
        public string IrisDetail { get; set; }
        public string Pupil { get; set; }
        public string Chrystal { get; set; }
        public string ChrystalDetail { get; set; }
        public string OcularFundus { get; set; }
        public int Pressure { get; set; }

        public Eye() { }

        public Eye(EyeSide side)
        {
            //EyeSide = side;
            EyeSideId = side.Id;
            EyeBallPosition = "Правильное";
            Motion = "В полном объеме";
            Character = "Спокойны";
            EyeLids = "Не изменены";
            EyeLashes = "Правильный";
            Palpation = "Отделяемого нет";
            Conjunctiva = "Нет";
            Cornea = "Прозрачная";
            Moisture = "Чистая";
            Iris = "Прозрачная";
            Chrystal = "Прозрачный";
        }

        public int CompareTo(object obj)
        {
            Eye newEye = obj as Eye;
            if (newEye != null)
            {
                if ((this.EyeBallPosition == newEye.EyeBallPosition) &&
                    (this.EyeBallPositionDetail == newEye.EyeBallPositionDetail) &&
                    (this.Motion == newEye.Motion) &&
                    (this.MotionDetail == newEye.MotionDetail) &&
                    (this.Character == newEye.Character) &&
                    (this.CharacterDetail == newEye.CharacterDetail) &&
                    (this.EyeLids == newEye.EyeLids) &&
                    (this.EyeLidsDetail == newEye.EyeLidsDetail) &&
                    (this.EyeLashes == newEye.EyeLashes) &&
                    (this.EyeLashesDetail == newEye.EyeLashesDetail) &&
                    (this.Palpation == newEye.Palpation) &&
                    (this.PalpationDetail == newEye.PalpationDetail) &&
                    (this.Conjunctiva == newEye.Conjunctiva) &&
                    (this.ConjunctivaDetail == newEye.ConjunctivaDetail) &&
                    (this.Cornea == newEye.Cornea) &&
                    (this.CorneaDetail == newEye.CorneaDetail) &&
                    (this.FrontCamera == newEye.FrontCamera) &&
                    (this.Moisture == newEye.Moisture) &&
                    (this.MoistureDetail == newEye.MoistureDetail) &&
                    (this.FrontCameraAngle == newEye.FrontCameraAngle) &&
                    (this.Iris == newEye.Iris) &&
                    (this.IrisDetail == newEye.IrisDetail) &&
                    (this.Pupil == newEye.Pupil) &&
                    (this.Chrystal == newEye.Chrystal) &&
                    (this.ChrystalDetail == newEye.ChrystalDetail))
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                throw new ArgumentException("Невозможно сравнить два объекта глаза", nameof(newEye));
            }
        }

        public void Equate(Eye eye)
        {
            this.EyeBallPosition = eye.EyeBallPosition;
            this.EyeBallPositionDetail = eye.EyeBallPositionDetail;
            this.Motion = eye.Motion;
            this.MotionDetail = eye.MotionDetail;
            this.Character = eye.Character;
            this.CharacterDetail = eye.CharacterDetail;
            this.EyeLids = eye.EyeLids;
            this.EyeLidsDetail = eye.EyeLidsDetail;
            this.EyeLashes = eye.EyeLashes;
            this.EyeLashesDetail = eye.EyeLashesDetail;
            this.Palpation = eye.Palpation;
            this.PalpationDetail = eye.PalpationDetail;
            this.Conjunctiva = eye.Conjunctiva;
            this.ConjunctivaDetail = eye.ConjunctivaDetail;
            this.Cornea = eye.Cornea;
            this.CorneaDetail = eye.CorneaDetail;
            this.FrontCamera = eye.FrontCamera;
            this.Moisture = eye.Moisture;
            this.MoistureDetail = eye.MoistureDetail;
            this.FrontCameraAngle = eye.FrontCameraAngle;
            this.Iris = eye.Iris;
            this.IrisDetail = eye.IrisDetail;
            this.Pupil = eye.Pupil;
            this.Chrystal = eye.Chrystal;
            this.ChrystalDetail = eye.ChrystalDetail;
        }

        public void Copy(Eye eye)
        {
            Visus = eye.Visus;

            this.Equate(eye);

            OcularFundus = eye.OcularFundus;
            Pressure = eye.Pressure;
        }
    }
}
