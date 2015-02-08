class RemindersController < ApplicationController

   before_filter :authenticate_user!
  
  # GET /reminders
  # GET /reminders.json
  def index
    @reminders = Reminder.all

    respond_to do |format|
      format.html # index.html.erb
      format.json { render json: @reminders }
    end
  end

  # GET /reminders/1
  # GET /reminders/1.json
  def show
    @reminder = Reminder.find(params[:id])

    respond_to do |format|
      format.html # show.html.erb
      format.json { render json: @reminder }
    end
  end

  # GET /reminders/new
  # GET /reminders/new.json
  def new
    @reminder = Reminder.new

    respond_to do |format|
      format.html # new.html.erb
      format.json { render json: @reminder }
    end
  end

  # GET /reminders/1/edit
  def edit
    @reminder = Reminder.find(params[:id])
  end

  # POST /reminders
  # POST /reminders.json
  def create
    @reminder = Reminder.new(params[:reminder])

    respond_to do |format|
      if @reminder.save
        format.html { redirect_to @reminder, notice: 'Reminder was successfully created.' }
        format.json { render json: @reminder, status: :created, location: @reminder }
      else
        format.html { render action: "new" }
        format.json { render json: @reminder.errors, status: :unprocessable_entity }
      end
    end
  end

  # PUT /reminders/1
  # PUT /reminders/1.json
  def update
    @reminder = Reminder.find(params[:id])

    respond_to do |format|
      if @reminder.update_attributes(params[:reminder])
        format.html { redirect_to @reminder, notice: 'Reminder was successfully updated.' }
        format.json { head :no_content }
      else
        format.html { render action: "edit" }
        format.json { render json: @reminder.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /reminders/1
  # DELETE /reminders/1.json
  def destroy
    @reminder = Reminder.find(params[:id])
    @reminder.destroy

    respond_to do |format|
      format.html { redirect_to reminders_url }
      format.json { head :no_content }
    end
  end

  def send_email_reminder
    @reminder = Reminder.find(params[:id])
    @user = current_user
    PillboxMailer.reminder_email(@user, @reminder).deliver
    render :json => @reminder
   end

  def send_text_reminder
    @reminder = Reminder.find(params[:id])
    @user = current_user
    reminder_text(@user, @reminder)
    render :json => @reminder
  end

  def send_voice_reminder
    @reminder = Reminder.find(params[:id])
    @user = current_user
    reminder_voice(@user, @reminder)
    render :json => @reminder
  end

  def reminder_call
    @post_to =  "/directions"
    render :action => "reminder.xml.builder", :layout => false
  end

  def directions
    if params['Digits'] == '1'
      redirect_to :action => 'reminder_call'
      return
    end

    if params['Digits'] == '2'
      redirect_to :action => 'goodbye'
      return
    end

    @redirect_to = "/goodbye"
    render :action => "goodbye.xml.builder", :layout => false 
  end

  def goodbye
    render :action => "goodbye.xml.builder", :layout => false 
  end
end
