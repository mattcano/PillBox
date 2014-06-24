class AddAddedByToMedications < ActiveRecord::Migration
  def change
    add_column :medications, :added_by, :integer
  end
end
