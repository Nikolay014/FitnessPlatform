using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.GCommon
{
    public static class ValidationConstraints
    {
        public static class GymConst 
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 3;

            public const int LocationMaxLength = 80;
            public const int LocationMinLength = 4;

            public const int DescriptionMaxLength = 350;
        }
        public static class ApplicationUserConst
        {
            public const int FirstNameMaxLength = 50;

            public const int LastNameMaxLength = 50;

            public const int GenderMaxLength = 10;

            public const int PhoneNumberMaxLength = 12;

            public const int ImageURLMaxLength = 300;

            public const int HaightMin = 50;

            public const int HaightMax = 250;

            public const int WeightMin = 20;

            public const int WeightMax = 300;
        }

        public static class TrainerConst
        {
            public const int ImageURLMaxLength = 300;
        }

        public static class EventConst
        {
            public const int TitleMaxLength = 100;
            public const int TitleMinLength = 3;


            public const int ImageURLMaxLength = 300;

            public const int DescriptionMaxLength = 500;

           
        }
        public static class FoodConst
        {
            
            public const int DescriptionMaxLength = 300;

        }

        public static class GymImageConst
        {

            public const int ImageURLMaxLength = 300;

        }

        public static class SpecialtyConst
        {

            public const int NameMaxLength = 100;
            public const int DescriptionMaxLength = 500;

        }
        public static class SubscriptionPlanConst
        {

            public const int NameMaxLength = 30;

            public const int DescriptionMaxLength = 500;
        }

        public static class WorkoutEntryConst
        {

            public const int ExerciseMaxLength = 100;

            
        }
        public static class WorkoutSessionConst
        {

            public const int NotesMaxLength = 500;


        }




    }
}
