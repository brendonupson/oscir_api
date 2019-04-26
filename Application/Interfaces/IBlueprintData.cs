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
        bool DeleteClass(Guid classGuid);

        IEnumerable<ClassEntity> ReadClasses(Guid[] classGuids);
        IEnumerable<ClassEntity> ReadClasses(string classNameContains, string classCategoryEquals);
        ClassEntity GetClassFullPropertyDefinition(Guid classGuid);


        ClassExtendEntity CreateClassExtend(ClassExtendEntity classExtendEntity);
        //ClassExtendEntity ReadClassExtend(Guid classExtendGuid);
        //ClassExtendEntity UpdateClassExtend(ClassExtendEntity classExtendEntity);
        bool DeleteClassExtend(Guid classExtendGuid);

        ClassRelationshipEntity CreateClassRelationship(ClassRelationshipEntity classRelationshipEntity);
        ClassRelationshipEntity ReadClassRelationship(Guid classRelationshipGuid);
        //ClassRelationshipEntity UpdateClassRelationship(ClassRelationshipEntity classRelationshipEntity);
        bool DeleteClassRelationship(Guid classRelationshipGuid);

        ClassRelationshipEntity GetClassRelationship(Guid sourceClassEntityId, Guid targetClassEntityId, string relationshipHint);


        ClassPropertyEntity CreateClassProperty(ClassPropertyEntity classPropertyEntity);
        //ClassPropertyEntity ReadClassProperty(Guid classPropertyGuid);
        ClassPropertyEntity UpdateClassProperty(ClassPropertyEntity classPropertyEntity);
        bool DeleteClassProperty(Guid classPropertyGuid);

        //IEnumerable<OwnerEntity> GetOwners(string ownerCodeContains, string ownerNameContains);

    }
}
