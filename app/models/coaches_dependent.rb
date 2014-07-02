class CoachesDependent < ActiveRecord::Base
  # attr_accessible :coach_id, :dependent_id

  belongs_to :coach, :class_name => "User"
  belongs_to :dependent, :class_name => "User"

end
