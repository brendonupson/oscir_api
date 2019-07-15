using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSCiR.Model;

namespace Application.Interfaces
{
    public interface IBlueprintData
    {
        ClassEntity CreateClass(ClassEntity classEntity);
        //ClassEntity ReadClass(Guid classGuid);
        ClassEntity UpdateClass(ClassEntity classEntity);
        bool DeleteClass(Guid classGuid, string userName);

        IEnumerable<ClassEntity> ReadClasses(Guid[] classGuids);
        IEnumerable<ClassEntity> ReadClasses(string classNameContains, string classNameEquals, string classCategoryEquals, bool getUsedClassesOnly);
        ClassEntity GetClassFullPropertyDefinition(Guid classGuid);


        ClassExtendEntity CreateClassExtend(ClassExtendEntity classExtendEntity);
        bool DeleteClassExtend(Guid classExtendGuid);

        ClassRelationshipEntity CreateClassRelationship(ClassRelationshipEntity classRelationshipEntity);
        ClassRelationshipEntity ReadClassRelationship(Guid classRelationshipGuid);
         bool DeleteClassRelationship(Guid classRelationshipGuid, string userName);

        ClassRelationshipEntity GetClassRelationship(Guid sourceClassEntityId, Guid targetClassEntityId, string relationshipHint);


        ClassPropertyEntity CreateClassProperty(ClassPropertyEntity classPropertyEntity);
         ClassPropertyEntity UpdateClassProperty(ClassPropertyEntity classPropertyEntity);
        bool DeleteClassProperty(Guid classPropertyGuid);

    }
}
